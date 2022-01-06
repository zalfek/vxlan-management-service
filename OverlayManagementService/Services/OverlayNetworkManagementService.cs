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

        public void DeleteNetwork(string groupId)
        {
            _logger.LogInformation("Searching for group id " + groupId + " in network repository");
            IOverlayNetwork overlayNetwork = _networkRepository.GetOverlayNetwork(groupId);
            _logger.LogInformation("Initiating network Clean up");
            overlayNetwork.CleanUpNetwork();
            _logger.LogInformation("Deleting network from repository");
            _networkRepository.DeleteOverlayNetwork(groupId);
        }

        public IOverlayNetwork DeployNetwork(OVSConnection oVSConnection)
        {
            _logger.LogInformation("Searching for switch with key: " + oVSConnection.Key + " in switch repository");
            IOpenVirtualSwitch openVirtualSwitch = _switchRepository.GetSwitch(oVSConnection.Key);
            string vni = _vniResolver.GenerateUniqueVNI();
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

         public IEnumerable<IOverlayNetwork> GetAllNetworks()
        {
            _logger.LogInformation("Getting networks from repository");
            return _networkRepository.GetAllNetworks().Values.ToArray();
        }

        public IOverlayNetwork GetNetworkByVni(string vni)
        {
            _logger.LogInformation("Searching for network id " + vni + " in network repository");
            return _networkRepository.GetOverlayNetworkByVni(vni);
        }

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
