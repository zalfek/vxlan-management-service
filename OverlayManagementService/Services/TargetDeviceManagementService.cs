using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using OverlayManagementService.Dtos;
using OverlayManagementService.Factories;
using OverlayManagementService.Network;
using OverlayManagementService.Repositories;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace OverlayManagementService.Services
{
    /// <summary>
    /// Service class(Facade) which acts as interface for all target device management related operations.
    /// </summary>
    public class TargetDeviceManagementService : ITargetDeviceManagementService
    {

        private readonly ILogger<TargetDeviceManagementService> _logger;
        private readonly INetworkRepository _networkRepository;
        private readonly ITargetDeviceFactory _targetDeviceFactory;
        private readonly IConfiguration _configuration;
        private readonly IKeyKeeper _keyKeeper;


        /// <summary>
        /// Constructor for TargetDeviceManagementService.
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="networkRepository">Network repository object</param>
        /// <param name="configuration">Configuration that can be used to obtain important information</param>
        /// <param name="targetDeviceFactory">Factory object which will be responsible for creation of TargetDevice objects</param>
        /// <param name="keyKeeper">KeyKeeper object</param>
        /// <returns>new TargetDeviceManagementService object</returns>
        public TargetDeviceManagementService(
            ILogger<TargetDeviceManagementService> logger,
            INetworkRepository networkRepository,
            ITargetDeviceFactory targetDeviceFactory,
            IConfiguration configuration,
            IKeyKeeper keyKeeper
            )
        {
            _logger = logger;
            _networkRepository = networkRepository;
            _targetDeviceFactory = targetDeviceFactory;
            _configuration = configuration;
            _keyKeeper = keyKeeper;
        }

        /// <summary>
        /// Method that that triggers all events that are required to deploy the target device.
        /// </summary>
        /// <param name="vmConnection">VmConnection DTO that holds all necessary data for device deployment</param>
        /// <returns>OverlayNetwork object</returns>
        public IOverlayNetwork RegisterMachine(VmConnection vmConnection)
        {
            Guid guid = Guid.NewGuid();
            _keyKeeper.PutKey(guid.ToString(), vmConnection.KeyFile);
            _logger.LogInformation("Searching for group id " + vmConnection.GroupId + " in network repository");
            IOverlayNetwork overlayNetwork = _networkRepository.GetOverlayNetwork(vmConnection.GroupId);
            ITargetDevice virtualMachine = _targetDeviceFactory.CreateTargetDevice(
                guid,
                _configuration["LinuxUsernames:TargetUsername"],
                overlayNetwork.OpenVirtualSwitch.Key,
                vmConnection.ManagementIp,
                overlayNetwork.Vni,
                overlayNetwork.OpenVirtualSwitch.PrivateIP,
                vmConnection.CommunicationIP
                );
            _logger.LogInformation("Adding device with id: " + virtualMachine.Guid + " to network with group id: " + overlayNetwork.GroupId);
            overlayNetwork.AddTargetDevice(virtualMachine);
            _logger.LogInformation("Saving updated newtwork to repository");
            _networkRepository.SaveOverlayNetwork(overlayNetwork);
            return overlayNetwork;
        }

        /// <summary>
        /// Method that that triggers all events that are required to remove the target device.
        /// </summary>
        /// <param name="groupId">Id of a group to which bellongs the network to which device was previously deployed</param>
        /// <param name="guid">Id which was assigned to device on deployment</param>
        public IOverlayNetwork UnRegisterMachine(string groupId, Guid guid)
        {
            _logger.LogInformation("Searching for group id " + groupId + " in network repository");
            IOverlayNetwork overlayNetwork = _networkRepository.GetOverlayNetwork(groupId);
            _logger.LogInformation("Removing Machine "+ guid + " from network");
            overlayNetwork.RemoveTargetDevice(guid);
            _logger.LogInformation("Saving updated newtwork to repository");
            _networkRepository.SaveOverlayNetwork(overlayNetwork);
            return overlayNetwork;
        }
    }
}
