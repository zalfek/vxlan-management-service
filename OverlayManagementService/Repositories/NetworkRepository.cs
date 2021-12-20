using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OverlayManagementService.Dtos;
using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OverlayManagementService.Repositories
{
    public class NetworkRepository : INetworkRepository
    {

        private readonly ILogger<INetworkRepository> _logger;
        private static IDictionary<string, IOverlayNetwork> _dbMock;
        public NetworkRepository()
        {
            _logger = new LoggerFactory().CreateLogger<INetworkRepository>();
            _dbMock = new Dictionary<string, IOverlayNetwork>();
        }

        public void DeleteOverlayNetwork(string groupId)
        {
            _logger.LogInformation("Removing network with id: " + groupId + " from database");
            _dbMock.Remove(groupId);
        }

        public IOverlayNetwork GetOverlayNetwork(string groupId)
        {
            return _dbMock[groupId];
        }

        public IOverlayNetwork SaveOverlayNetwork(IOverlayNetwork overlayNetwork)
        {
            if (_dbMock.ContainsKey(overlayNetwork.GroupId))
            {
                _logger.LogInformation("Network with id: " + overlayNetwork.GroupId + " already exists. Updating the state in database");
                _dbMock[overlayNetwork.GroupId] = overlayNetwork;
            }
            else
            {
                _logger.LogInformation("Network with id: " + overlayNetwork.GroupId + " does not exists. Creating new entry in database");
                _dbMock.Add(overlayNetwork.GroupId, overlayNetwork);
            }
            return overlayNetwork;
        }

        public IOverlayNetwork UpdateOverlayNetwork(IOverlayNetwork overlayNetwork)
        {
            _logger.LogInformation("Updating the state of network in database");
            _dbMock[overlayNetwork.GroupId] = overlayNetwork;
            return overlayNetwork;
        }

        public IDictionary<string, IOverlayNetwork> GetAllNetworks()
        {
            return _dbMock;
        }

        public IOverlayNetwork GetOverlayNetworkByVni(string vni)
        {
            foreach (KeyValuePair<string, IOverlayNetwork> keyValuePair in _dbMock)
            {
                if (keyValuePair.Value.Vni == vni) { 
                    return keyValuePair.Value; 
                }
            }
            throw new KeyNotFoundException();
        }

        public IOverlayNetwork GetOverlayNetwork(Claim claim)
        {
            if (_dbMock.ContainsKey(claim.Value))
            {
                return _dbMock[claim.Value];
            }
            else { return null; }
        }
    }
}
