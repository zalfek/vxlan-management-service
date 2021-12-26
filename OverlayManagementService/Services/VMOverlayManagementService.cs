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
    public class VMOverlayManagementService : IOverlayManagementService
    {

        private readonly ILogger<VMOverlayManagementService> _logger;
        private readonly INetworkRepository _networkRepository;
        private readonly ISwitchRepository _switchRepository;
        private readonly IIdentifier _vniResolver;
        private readonly IIdentifierFactory<IPAddress> _ipResolverFactory;
        private readonly INetworkFactory _networkFactory;
        private readonly IBridgeFactory _bridgeFactory;
        private readonly IVirtualMachineFactory _virtualMachineFactory;
        private readonly IFirewallRepository _firewallRepository;
        private readonly IFirewallFactory _firewallFactory;
        private readonly IOpenVirtualSwitchFactory _openVirtualSwitchFactory;
        private readonly IKeyKeeper _keyKeeper;
        private readonly IConfiguration _configuration;



        public VMOverlayManagementService(
            ILogger<VMOverlayManagementService> logger,
            INetworkRepository networkRepository,
            IIdentifier vniResolver,
            INetworkFactory networkFactory,
            IBridgeFactory bridgeFactory,
            IVirtualMachineFactory virtualMachineFactory,
            IIdentifierFactory<IPAddress> ipResolverFactory,
            ISwitchRepository switchRepository,
            IFirewallRepository firewallRepository,
            IFirewallFactory firewallFactory,
            IOpenVirtualSwitchFactory openVirtualSwitchFactory,
            IKeyKeeper keyKeeper,
            IConfiguration configuration
            )
        {
            _logger = logger;
            _networkRepository = networkRepository;
            _vniResolver = vniResolver;
            _ipResolverFactory = ipResolverFactory;
            _networkFactory = networkFactory;
            _bridgeFactory = bridgeFactory;
            _virtualMachineFactory = virtualMachineFactory;
            _switchRepository = switchRepository;
            _firewallRepository = firewallRepository;
            _firewallFactory = firewallFactory;
            _openVirtualSwitchFactory = openVirtualSwitchFactory;
            _keyKeeper = keyKeeper;
            _configuration = configuration;
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
            if (oVSConnection.vmConnection != null) {
                _logger.LogInformation("Registering new device in the network");
                RegisterMachine(oVSConnection.vmConnection);
            }
            return overlayNetwork;
        }

        public IOverlayNetwork RegisterMachine(VmConnection vmConnection)
        {
            _logger.LogInformation("Searching for group id " + vmConnection.GroupId + " in network repository");
            IOverlayNetwork overlayNetwork = _networkRepository.GetOverlayNetwork(vmConnection.GroupId);
            IVirtualMachine virtualMachine = _virtualMachineFactory.CreateVirtualMachine(
                Guid.NewGuid(),
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

        public IOpenVirtualSwitch AddSwitch(OvsRegistration ovsRegistration)
        {
            _keyKeeper.PutKey(ovsRegistration.Key, ovsRegistration.KeyFile);
            IOpenVirtualSwitch openVirtualSwitch = _openVirtualSwitchFactory.CreateSwitch(ovsRegistration);
            _logger.LogInformation("Saving new switch to repository");
            _switchRepository.SaveSwitch(openVirtualSwitch);
            _logger.LogInformation("Creating new Firewall entry");
            IFirewall firewall = _firewallFactory.CreateFirewall(openVirtualSwitch.ManagementIp);
            _logger.LogInformation("Saving new Firewall");
            _firewallRepository.AddFirewall(openVirtualSwitch.Key, firewall);
            return openVirtualSwitch;
        }

        public void RemoveSwitch(string key)
        {
            _logger.LogInformation("Removing switch from repository");
            _switchRepository.DeleteSwitch(key);
            _logger.LogInformation("Removing Firewalll from repository");
            _firewallRepository.RemoveFirewall(key);
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

        public IEnumerable<IOpenVirtualSwitch> GetAllSwitches()
        {
            _logger.LogInformation("Getting switches from repository");
            return _switchRepository.GetAllSwitches().Values.ToArray();
        }

        public IOpenVirtualSwitch GetSwitch(string key)
        {
            _logger.LogInformation("Searching for " + key + " in Switch repository");
            return _switchRepository.GetSwitch(key);
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
