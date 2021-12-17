using Microsoft.Extensions.Logging;
using OverlayManagementService.Dtos;
using OverlayManagementService.Factories;
using OverlayManagementService.Network;
using OverlayManagementService.Repositories;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OverlayManagementService.Services
{
    public class VMOverlayConnectionService : IOverlayConnectionService
    {

        private readonly INetworkRepository _jsonRepository;
        private readonly ILogger<VMOverlayConnectionService> _logger;
        private readonly IClientConnectionFactory _clientConnectionFactory;
        private readonly IFirewallRepository _firewallRepository;

        public VMOverlayConnectionService(INetworkRepository jsonRepository, ILogger<VMOverlayConnectionService> logger, IClientConnectionFactory clientConnectionFactory, IFirewallRepository firewallRepository)
        {
            _jsonRepository = jsonRepository;
            _logger = logger;
            _clientConnectionFactory = clientConnectionFactory;
            _firewallRepository = firewallRepository;
        }

        public IEnumerable<ClientConnection> GetAllNetworks(IEnumerable<Claim> claims)
        {
            List<ClientConnection> networks = new();
            _logger.LogInformation("Searching for assigned networks");
            foreach (var claim in claims)
            {
                IOverlayNetwork network = _jsonRepository.GetOverlayNetwork(claim);
                if (network != null) {
                    _logger.LogInformation("Network found: " + network.ToString());
                    ClientConnection clientConnection = _clientConnectionFactory.CreateClientConnectionDto(network.Vni, claim.Value, network.OpenVirtualSwitch.PublicIP, null);
                    networks.Add(clientConnection);
                }
            }
            return networks;
        }

        public ClientConnection CreateConnection(string groupId, string ip)
        {
            _logger.LogInformation("Searching requested network");
            IOverlayNetwork overlayNetwork = _jsonRepository.GetOverlayNetwork(groupId);
            _logger.LogInformation("Initiating connection on Open Virtual Switch");
            string clientIp = overlayNetwork.AddClient(ip);
            IFirewall firewall = _firewallRepository.GetFirewall(overlayNetwork.OpenVirtualSwitch.Key);
            _logger.LogInformation("Creating temporary exception for client");
            firewall.AddException(clientIp);
            return _clientConnectionFactory.CreateClientConnectionDto(overlayNetwork.Vni, groupId, overlayNetwork.OpenVirtualSwitch.PublicIP, clientIp);
        }
    }
}
