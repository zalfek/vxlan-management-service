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
    public class SwitchManagementService : ISwitchManagementService
    {

        private readonly ILogger<SwitchManagementService> _logger;
        private readonly INetworkRepository _networkRepository;
        private readonly ISwitchRepository _switchRepository;
        private readonly IFirewallRepository _firewallRepository;
        private readonly IFirewallFactory _firewallFactory;
        private readonly IOpenVirtualSwitchFactory _openVirtualSwitchFactory;
        private readonly IKeyKeeper _keyKeeper;

        public SwitchManagementService(
            ILogger<SwitchManagementService> logger,
            INetworkRepository networkRepository,
            ISwitchRepository switchRepository,
            IFirewallRepository firewallRepository,
            IFirewallFactory firewallFactory,
            IOpenVirtualSwitchFactory openVirtualSwitchFactory,
            IKeyKeeper keyKeeper
            )
        {
            _logger = logger;
            _networkRepository = networkRepository;
            _switchRepository = switchRepository;
            _firewallRepository = firewallRepository;
            _firewallFactory = firewallFactory;
            _openVirtualSwitchFactory = openVirtualSwitchFactory;
            _keyKeeper = keyKeeper;
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
            foreach (var network in _networkRepository.GetAllNetworks().Values.ToArray())
            {
                if(network.OpenVirtualSwitch.Key == key) {
                    _logger.LogInformation("Initiating network Clean up");
                    network.CleanUpNetwork();
                    _logger.LogInformation("Deleting network from repository");
                    _networkRepository.DeleteOverlayNetwork(network.GroupId);
                }
            } 
            _logger.LogInformation("Removing switch from repository");
            _switchRepository.DeleteSwitch(key);
            _logger.LogInformation("Removing Firewalll from repository");
            _firewallRepository.RemoveFirewall(key);
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
    }
}
