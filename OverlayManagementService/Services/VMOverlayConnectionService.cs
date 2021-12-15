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
        private readonly IFirewall _firewall;
        private readonly IAddress _ipAddress;
        private readonly IClientConnectionFactory _clientConnectionFactory;

        public VMOverlayConnectionService(INetworkRepository jsonRepository, ILogger<VMOverlayConnectionService> logger, IAddress ipAddress, IClientConnectionFactory clientConnectionFactory)
        {
            _jsonRepository = jsonRepository;
            _logger = logger;
            _ipAddress = ipAddress;
            _clientConnectionFactory = clientConnectionFactory;
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
            overlayNetwork.AddClient(ip);
            return _clientConnectionFactory.CreateClientConnectionDto(overlayNetwork.Vni, groupId, overlayNetwork.OpenVirtualSwitch.PublicIP, _ipAddress.GenerarteUniqueIPV4Address());
        }
    }
}
