using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OverlayManagementService.Dtos;
using OverlayManagementService.Network;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OverlayManagementService.Repositories
{

    /// <summary>
    /// Class encapsulates the logic reguired to persist Network objects.
    /// </summary>
    public class NetworkRepository : INetworkRepository
    {

        private readonly ILogger<INetworkRepository> _logger;
        private static ConcurrentDictionary<string, IOverlayNetwork> _dbMock;
        private readonly string _backupPath;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        public NetworkRepository()
        {
            _logger = LoggerFactory.Create(logging => logging.AddConsole()).CreateLogger<INetworkRepository>();
            _backupPath = Path.Combine(AppContext.BaseDirectory, "Resources", "NetworkData.json");
            _jsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            _dbMock = JsonConvert.DeserializeObject<ConcurrentDictionary<string, IOverlayNetwork>>(File.ReadAllText(_backupPath), _jsonSerializerSettings);
        }

        /// <summary>
        /// Method allows to remove the Network object from in memory database for the specific group id.
        /// </summary>
        /// <param name="groupId">Id of Azure Active Derictory group for which Network was deployed</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteOverlayNetwork(string groupId)
        {
            _logger.LogInformation("Removing network with id: " + groupId + " from database");
            _dbMock.TryRemove(groupId, out _);
            DoBackup();
        }

        /// <summary>
        /// Method allows to get the Network object from in memory database for the specific group id.
        /// </summary>
        /// <param name="groupId">Id of Azure Active Derictory group for which Network was deployed</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IOverlayNetwork GetOverlayNetwork(string groupId)
        {
            _dbMock.TryGetValue(groupId, out IOverlayNetwork value);
            return value;
        }

        /// <summary>
        /// Method allows to save the Network object to in memory database for the specific group id.
        /// </summary>
        /// <param name="overlayNetwork">OverlayNetwork object</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IOverlayNetwork SaveOverlayNetwork(IOverlayNetwork overlayNetwork)
        {
            if (_dbMock.ContainsKey(overlayNetwork.GroupId))
            {
                _logger.LogInformation("Network with id: " + overlayNetwork.GroupId + " already exists. Updating the state in database");
                _dbMock.TryGetValue(overlayNetwork.GroupId, out IOverlayNetwork value);
                _dbMock.TryUpdate(overlayNetwork.GroupId, overlayNetwork, value);
            }
            else
            {
                _logger.LogInformation("Network with id: " + overlayNetwork.GroupId + " does not exists. Creating new entry in database");
                _dbMock.TryAdd(overlayNetwork.GroupId, overlayNetwork);
            }
            DoBackup();
            return overlayNetwork;
        }

        /// <summary>
        /// Method allows to update exiing Network object in in memory database for the specific group id.
        /// </summary>
        /// <param name="overlayNetwork">OverlayNetwork object</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IOverlayNetwork UpdateOverlayNetwork(IOverlayNetwork overlayNetwork)
        {
            _logger.LogInformation("Updating the state of network in database");
            _dbMock.TryGetValue(overlayNetwork.GroupId, out IOverlayNetwork value);
            _dbMock.TryUpdate(overlayNetwork.GroupId, overlayNetwork, value);
            DoBackup();
            return overlayNetwork;
        }

        /// <summary>
        /// Method allows to get all deployed Network objects from in memory database.
        /// </summary>
        /// <returns>Dictionary with maping Azure AD group to Overlay Network</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IDictionary<string, IOverlayNetwork> GetAllNetworks()
        {
            return _dbMock;
        }

        /// <summary>
        /// Method allows to get the Network object from in memory database with specific VNI.
        /// </summary>
        /// <param name="vni">VNI of the network</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
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

        /// <summary>
        /// Method allows to get the Network object from in memory database for the specific group claim.
        /// </summary>
        /// <param name="claim">claim from the access token</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IOverlayNetwork GetOverlayNetwork(Claim claim)
        {
            if (_dbMock.ContainsKey(claim.Value))
            {
                _dbMock.TryGetValue(claim.Value, out IOverlayNetwork value);
                return value;
            }
            else { return null; }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void DoBackup() {
            File.WriteAllText(_backupPath, JsonConvert.SerializeObject(_dbMock, _jsonSerializerSettings));
        }


    }
}
