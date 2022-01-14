using Microsoft.Extensions.Logging;
using OverlayManagementService.Dtos;
using OverlayManagementService.Factories;
using OverlayManagementService.Network;
using OverlayManagementService.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace OverlayManagementService.Services
{

    /// <summary>
    /// Service class(Facade) which acts as interface for all Switch management related operations.
    /// </summary>
    public class SwitchManagementService : ISwitchManagementService
    {

        private readonly ILogger<SwitchManagementService> _logger;
        private readonly INetworkRepository _networkRepository;
        private readonly ISwitchRepository _switchRepository;
        private readonly IFirewallRepository _firewallRepository;
        private readonly IFirewallFactory _firewallFactory;
        private readonly IOpenVirtualSwitchFactory _openVirtualSwitchFactory;
        private readonly IKeyKeeper _keyKeeper;

        /// <summary>
        /// Constructor for SwitchManagementService.
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="networkRepository">Network repository object</param>
        /// <param name="switchRepository">Switch repository object</param>
        /// <param name="firewallRepository">Firewall repository object</param>
        /// <param name="firewallFactory">Factory object which will be responsible for creation of Firewall objects</param>
        /// <param name="openVirtualSwitchFactory">Factory object which will be responsible for creation of Open Virtual Switch objects</param>
        /// <param name="keyKeeper">KeyKeeper object</param>
        /// <returns>new SwitchManagementService object</returns>
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

        /// <summary>
        /// Method that that triggers all events that are required to deploy the switch.
        /// </summary>
        /// <param name="ovsRegistration">OvsRegistration DTO that holds all necessary data for switch deployment</param>
        /// <returns>OpenVirtualSwitch object</returns>
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

        /// <summary>
        /// Method that that triggers all events that are required to remove the switch.
        /// </summary>
        /// <param name="key">Switch prefix under which Switch should be registered f.e "thu"</param>
        public void RemoveSwitch(string key)
        {
            foreach (var network in _networkRepository.GetAllNetworks().Values.ToArray())
            {
                if (network.OpenVirtualSwitch.Key == key)
                {
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

        /// <summary>
        /// Method for getting all deployed switches.
        /// </summary>
        /// <returns>IEnumerable containing  OpenVirtualSwitch objects</returns>
        public IEnumerable<IOpenVirtualSwitch> GetAllSwitches()
        {
            _logger.LogInformation("Getting switches from repository");
            return _switchRepository.GetAllSwitches().Values.ToArray();
        }

        /// <summary>
        /// Method for getting deployed switch with specific prefix.
        /// </summary>
        /// <param name="key">Switch prefix</param>
        /// <returns>OpenVirtualSwitch object</returns>
        public IOpenVirtualSwitch GetSwitch(string key)
        {
            _logger.LogInformation("Searching for " + key + " in Switch repository");
            return _switchRepository.GetSwitch(key);
        }
    }
}
