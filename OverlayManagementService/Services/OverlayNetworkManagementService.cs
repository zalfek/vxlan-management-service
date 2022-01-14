using Microsoft.Extensions.Logging;
using OverlayManagementService.Dtos;
using OverlayManagementService.Factories;
using OverlayManagementService.Network;
using OverlayManagementService.Repositories;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace OverlayManagementService.Services
{
    /// <summary>
    /// Service class(Facade) which acts as interface for all Network management related operations.
    /// </summary>
    public class OverlayNetworkManagementService : IOverlayNetworkManagementService
    {

        private readonly ILogger<OverlayNetworkManagementService> _logger;
        private readonly INetworkRepository _networkRepository;
        private readonly ISwitchRepository _switchRepository;
        private readonly IIdentifier _vniResolver;
        private readonly IIdentifierFactory<IPAddress> _ipResolverFactory;
        private readonly INetworkFactory _networkFactory;
        private readonly IBridgeFactory _bridgeFactory;
        private readonly IConfiguration _configuration;
        private readonly ITargetDeviceManagementService _targetDeviceManagementService;


        /// <summary>
        /// Constructor for OverlayNetworkManagementService.
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="networkRepository">Network repository object</param>
        /// <param name="vniResolver">Virtual Network Identifier object  which will be responsible for providing unique VNI's to networks</param>
        /// <param name="networkFactory">Factory object which will be responsible for creation of Network objects</param>
        /// <param name="bridgeFactory">Factory object which will be responsible for creation of Bridge objects</param>
        /// <param name="ipResolverFactory">Factory object which will be responsible for creation of Ip address resolver objects</param>
        /// <param name="switchRepository">Switch repository object</param>
        /// <param name="configuration">Configuration that can be used to obtain important information</param>
        /// <param name="targetDeviceManagementService">Target Device Management Service object which is responsible for removing target devices from network</param>
        /// <returns>new OverlayNetworkManagementService object</returns>
        public OverlayNetworkManagementService(
            ILogger<OverlayNetworkManagementService> logger,
            INetworkRepository networkRepository,
            IIdentifier vniResolver,
            INetworkFactory networkFactory,
            IBridgeFactory bridgeFactory,
            IIdentifierFactory<IPAddress> ipResolverFactory,
            ISwitchRepository switchRepository,
            IConfiguration configuration,
            ITargetDeviceManagementService targetDeviceManagementService
            )
        {
            _logger = logger;
            _networkRepository = networkRepository;
            _vniResolver = vniResolver;
            _ipResolverFactory = ipResolverFactory;
            _networkFactory = networkFactory;
            _bridgeFactory = bridgeFactory;
            _switchRepository = switchRepository;
            _configuration = configuration;
            _targetDeviceManagementService = targetDeviceManagementService;
        }

        /// <summary>
        /// Method that that triggers all events that are required to suspend the network.
        /// </summary>
        /// <param name="groupId">Id of a group to which the network is assigned</param>
        public void DeleteNetwork(string groupId)
        {
            _logger.LogInformation("Searching for group id " + groupId + " in network repository");
            IOverlayNetwork overlayNetwork = _networkRepository.GetOverlayNetwork(groupId);
            _logger.LogInformation("Initiating network Clean up");
            overlayNetwork.CleanUpNetwork();
            _vniResolver.ReleaseVNI(overlayNetwork.Vni);
            _logger.LogInformation("Deleting network from repository");
            _networkRepository.DeleteOverlayNetwork(groupId);
        }

        /// <summary>
        /// Method that that triggers all events that are required to deploy the network.
        /// </summary>
        /// <param name="oVSConnection">OVSConnection DTO that holds all necessary data for network deployment</param>
        /// <returns>OverlayNetwork object</returns>
        public IOverlayNetwork DeployNetwork(OVSConnection oVSConnection)
        {
            _logger.LogInformation("Searching for switch with key: " + oVSConnection.Key + " in switch repository");
            IOpenVirtualSwitch openVirtualSwitch = _switchRepository.GetSwitch(oVSConnection.Key);
            string vni = _vniResolver.ReserveVNI();
            _logger.LogInformation("New VNI generated: " + vni);
            _logger.LogInformation("Adding new bridge to OVS");
            openVirtualSwitch.AddBridge(
                _bridgeFactory.CreateBridge(
                    _configuration["LinuxUsernames:SwitchUsername"],
                    openVirtualSwitch.Key,
                    openVirtualSwitch.Key + "-vni-" + vni,
                    vni,
                    openVirtualSwitch.ManagementIp)
                );
            _logger.LogInformation("Creating new network with id: " + oVSConnection.GroupId);
            IOverlayNetwork overlayNetwork = _networkFactory.CreateOverlayNetwork(
                oVSConnection.GroupId,
                vni,
                openVirtualSwitch,
                _ipResolverFactory.CreateAddressResolver()
                );
            _logger.LogInformation("Saving new newtwork to repository");
            _networkRepository.SaveOverlayNetwork(overlayNetwork);
            _logger.LogInformation("Initiating network deployment");
            overlayNetwork.DeployNetwork();
            if (oVSConnection.VmConnection != null) {
                _logger.LogInformation("Registering new device in the network");
                _targetDeviceManagementService.RegisterMachine(oVSConnection.VmConnection);
            }
            return overlayNetwork;
        }

        /// <summary>
        /// Method for getting all deployed networks.
        /// </summary>
        /// <returns>IEnumerable containing  OverlayNetwork objects</returns>
        public IEnumerable<IOverlayNetwork> GetAllNetworks()
        {
            _logger.LogInformation("Getting networks from repository");
            return _networkRepository.GetAllNetworks().Values.ToArray();
        }

        /// <summary>
        /// Method for getting deployed network with specific VNI.
        /// </summary>
        /// <param name="vni">Virtual Network Identifier</param>
        /// <returns>OverlayNetwork object</returns>
        public IOverlayNetwork GetNetworkByVni(string vni)
        {
            _logger.LogInformation("Searching for network id " + vni + " in network repository");
            return _networkRepository.GetOverlayNetworkByVni(vni);
        }

        /// <summary>
        /// Method that that triggers all events that are required to update existing the network.
        /// TODO
        /// </summary>
        /// <param name="overlayNetwork">Overlay Network Object with contains required changes</param>
        /// <returns>OverlayNetwork object</returns>
        public IOverlayNetwork UpdateNetwork(IOverlayNetwork overlayNetwork)
        {
            _logger.LogInformation("Searching for original network in repository.");
            IOverlayNetwork oldNetwork = _networkRepository.GetOverlayNetwork(overlayNetwork.GroupId);
            _logger.LogInformation("Cleaning up old network");
            oldNetwork.CleanUpNetwork();
            _logger.LogInformation("Deploying updated network");
            overlayNetwork.DeployNetwork();
            _logger.LogInformation("Saving updated network");
            return _networkRepository.SaveOverlayNetwork(overlayNetwork);
        }
    }
}
