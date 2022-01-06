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
    public class TargetDeviceManagementService : ITargetDeviceManagementService
    {

        private readonly ILogger<TargetDeviceManagementService> _logger;
        private readonly INetworkRepository _networkRepository;
        private readonly IVirtualMachineFactory _virtualMachineFactory;
        private readonly IConfiguration _configuration;
        private readonly IKeyKeeper _keyKeeper;



        public TargetDeviceManagementService(
            ILogger<TargetDeviceManagementService> logger,
            INetworkRepository networkRepository,
            IVirtualMachineFactory virtualMachineFactory,
            IConfiguration configuration,
            IKeyKeeper keyKeeper
            )
        {
            _logger = logger;
            _networkRepository = networkRepository;
            _virtualMachineFactory = virtualMachineFactory;
            _configuration = configuration;
            _keyKeeper = keyKeeper;
        }

        public IOverlayNetwork RegisterMachine(VmConnection vmConnection)
        {
            Guid guid = Guid.NewGuid();
            _keyKeeper.PutKey(guid.ToString(), vmConnection.KeyFile);
            _logger.LogInformation("Searching for group id " + vmConnection.GroupId + " in network repository");
            IOverlayNetwork overlayNetwork = _networkRepository.GetOverlayNetwork(vmConnection.GroupId);
            IVirtualMachine virtualMachine = _virtualMachineFactory.CreateVirtualMachine(
                guid,
                _configuration["LinuxUsernames:TargetUsername"],
                overlayNetwork.OpenVirtualSwitch.Key,
                vmConnection.ManagementIp,
                overlayNetwork.Vni,
                overlayNetwork.OpenVirtualSwitch.PrivateIP,
                vmConnection.CommunicationIP
                );
            _logger.LogInformation("Adding device with id: " + virtualMachine.Guid + " to network with group id: " + overlayNetwork.GroupId);
            overlayNetwork.AddVMachine(virtualMachine);
            _logger.LogInformation("Saving updated newtwork to repository");
            _networkRepository.SaveOverlayNetwork(overlayNetwork);
            return overlayNetwork;
        }

        public IOverlayNetwork UnRegisterMachine(string groupId, Guid guid)
        {
            _logger.LogInformation("Searching for group id " + groupId + " in network repository");
            IOverlayNetwork overlayNetwork = _networkRepository.GetOverlayNetwork(groupId);
            _logger.LogInformation("Removing Machine "+ guid + " from network");
            overlayNetwork.RemoveVMachine(guid);
            _logger.LogInformation("Saving updated newtwork to repository");
            _networkRepository.SaveOverlayNetwork(overlayNetwork);
            return overlayNetwork;
        }
    }
}
