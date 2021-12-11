using Microsoft.Extensions.Logging;
using OverlayManagementService.Network;
using OverlayManagementService.Repositories;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OverlayManagementService.Services
{
    public class VMOverlayConnectionService : IOverlayConnectionService
    {

        private readonly IRepository _jsonRepository;
        private readonly ILogger<VMOverlayConnectionService> _logger;
        private readonly IFirewall _firewall;

        public VMOverlayConnectionService(IRepository jsonRepository, ILogger<VMOverlayConnectionService> logger)
        {
            this._jsonRepository = jsonRepository;
            this._logger = logger;
        }

        public IEnumerable<IOverlayNetwork> GetAllNetworks(IEnumerable<Claim> claims)
        {
            List<IOverlayNetwork> networks = new();
            _logger.LogInformation("Searching for assigned networks");
            foreach (var claim in claims)
            {
                IOverlayNetwork network = _jsonRepository.GetOverlayNetwork(claim);
                if (network != null) {
                    _logger.LogInformation("Network found: " + network.ToString());
                    networks.Add(network);
                }
            }
            return networks;
        }

        public IOverlayNetwork CreateConnection(string membership, string ip)
        {
            _logger.LogInformation("Searching requested network");
            IOverlayNetwork overlayNetwork = _jsonRepository.GetOverlayNetwork(membership);
            _logger.LogInformation("Initiating connection on Open Virtual Switch");
            overlayNetwork.AddClient(ip);
            return overlayNetwork;
        }
    }
}
