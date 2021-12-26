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
        private readonly string _backupPath;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        public NetworkRepository()
        {
            _logger = new LoggerFactory().CreateLogger<INetworkRepository>();
            _backupPath = Path.Combine(AppContext.BaseDirectory, "Resources", "NetworkData.json");
            _jsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            _dbMock = (IDictionary<string, IOverlayNetwork>)JsonConvert.DeserializeObject<Dictionary<string, IOverlayNetwork>>(File.ReadAllText(_backupPath), _jsonSerializerSettings);
        }

        public void DeleteOverlayNetwork(string groupId)
        {
            _logger.LogInformation("Removing network with id: " + groupId + " from database");
            _dbMock.Remove(groupId);
            File.WriteAllText(_backupPath, JsonConvert.SerializeObject(_dbMock, _jsonSerializerSettings));
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
            File.WriteAllText(_backupPath, JsonConvert.SerializeObject(_dbMock, _jsonSerializerSettings));
            return overlayNetwork;
        }

        public IOverlayNetwork UpdateOverlayNetwork(IOverlayNetwork overlayNetwork)
        {
            _logger.LogInformation("Updating the state of network in database");
            _dbMock[overlayNetwork.GroupId] = overlayNetwork;
            File.WriteAllText(_backupPath, JsonConvert.SerializeObject(_dbMock, _jsonSerializerSettings));
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
