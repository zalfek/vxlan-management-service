using Microsoft.Extensions.Logging;
using OverlayManagementService.Dtos;
using OverlayManagementService.Factories;
using OverlayManagementService.Models;
using OverlayManagementService.Network;
using OverlayManagementService.Repositories;
using System.Collections.Generic;
using System.Security.Claims;

namespace OverlayManagementService.Services
{
    /// <summary>
    /// Service class(Facade) which acts as interface for all Network connection related operations.
    /// </summary>
    public class OverlayNetworkConnectionService : IOverlayNetworkConnectionService
    {

        private readonly INetworkRepository _networkRepository;
        private readonly ILogger<OverlayNetworkConnectionService> _logger;
        private readonly IClientConnectionFactory _clientConnectionFactory;
        private readonly IFirewallRepository _firewallRepository;

        /// <summary>
        /// Constructor for OverlayNetworkConnectionService.
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="networkRepository">Network repository object</param>
        /// <param name="clientConnectionFactory">Factory object which will be responsible for creation of Client Connection DTO's</param>
        /// <param name="firewallRepository">Firewall repository object</param>
        /// <returns>new OverlayNetworkConnectionService object</returns>
        public OverlayNetworkConnectionService(INetworkRepository networkRepository, ILogger<OverlayNetworkConnectionService> logger, IClientConnectionFactory clientConnectionFactory, IFirewallRepository firewallRepository)
        {
            _networkRepository = networkRepository;
            _logger = logger;
            _clientConnectionFactory = clientConnectionFactory;
            _firewallRepository = firewallRepository;
        }

        /// <summary>
        /// Method that queries for assigned network based on provided claims.
        /// </summary>
        /// <param name="claims">List of claims</param>
        /// <returns>IEnumerable of ClientConnection objects</returns>
        public IEnumerable<ClientConnection> GetAllNetworks(IEnumerable<Claim> claims)
        {
            List<ClientConnection> networks = new();
            _logger.LogInformation("Searching for assigned networks");
            foreach (var claim in claims)
            {
                IOverlayNetwork network = _networkRepository.GetOverlayNetwork(claim);
                if (network != null)
                {
                    _logger.LogInformation("Network found: " + network.ToString());
                    ClientConnection clientConnection = _clientConnectionFactory.CreateClientConnectionDto(network.Vni, claim.Value, network.OpenVirtualSwitch.PublicIP, null);
                    networks.Add(clientConnection);
                }
            }
            return networks;
        }

        /// <summary>
        /// Method that preapares the data required for joining the specific network and creates a tunnel for client on switch side.
        /// </summary>
        /// <param name="groupId">Group id to which network is assigned</param>
        /// <param name="client">Student DTO</param>
        /// <returns>ClientConnection object containing information required for joining the network</returns>
        public ClientConnection CreateConnection(string groupId, Student client)
        {
            _logger.LogInformation("Searching network under group id: " + groupId);
            IOverlayNetwork overlayNetwork = _networkRepository.GetOverlayNetwork(groupId);
            _logger.LogInformation("Initiating connection on Open Virtual Switch");
            string clientVxlanIp = overlayNetwork.AddClient(client);
            IFirewall firewall = _firewallRepository.GetFirewall(overlayNetwork.OpenVirtualSwitch.Key);
            _logger.LogInformation("Creating temporary exception for client ip address: " + client.IpAddress);
            firewall.AddException(client.IpAddress);
            _networkRepository.SaveOverlayNetwork(overlayNetwork);
            return _clientConnectionFactory.CreateClientConnectionDto(overlayNetwork.Vni, groupId, overlayNetwork.OpenVirtualSwitch.PublicIP, clientVxlanIp);
        }


        /// <summary>
        /// Method that removes the connection to client from Open virtual switch side.
        /// </summary>
        /// <param name="groupId">Group id to which network is assigned</param>
        /// <param name="client">Student DTO</param>
        public void SuspendConnection(string groupId, Student client)
        {
            _logger.LogInformation("Searching network under group id: " + groupId);
            IOverlayNetwork overlayNetwork = _networkRepository.GetOverlayNetwork(groupId);
            _logger.LogInformation("Removing connection on Open Virtual Switch");
            overlayNetwork.RemoveClient(client);
            IFirewall firewall = _firewallRepository.GetFirewall(overlayNetwork.OpenVirtualSwitch.Key);
            _logger.LogInformation("Removing temporary exception for client ip address: " + client.IpAddress);
            firewall.RemoveException(client.IpAddress);
            _networkRepository.SaveOverlayNetwork(overlayNetwork);
        }


    }
}
