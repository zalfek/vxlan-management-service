using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OverlayManagementService.Network;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace OverlayManagementService.Repositories
{

    /// <summary>
    /// Class encapsulates the logic reguired to persist Firewall objects.
    /// </summary>
    public class FirewallRepository : IFirewallRepository
    {
        private readonly ILogger<IFirewallRepository> _logger;
        private static ConcurrentDictionary<string, IFirewall> _dbMock;
        private readonly string _backupPath;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        public FirewallRepository()
        {
            _logger = new LoggerFactory().CreateLogger<IFirewallRepository>();
            _backupPath = Path.Combine(AppContext.BaseDirectory, "Resources", "FirewallData.json");
            _jsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            _dbMock = JsonConvert.DeserializeObject<ConcurrentDictionary<string, IFirewall>>(File.ReadAllText(_backupPath), _jsonSerializerSettings);
        }

        /// <summary>
        /// Method allows to add firewall to the in memory database which is aafterward backed up into the json file.
        /// </summary>
        /// <param name="key">Open Virtual Switch prefix</param>
        /// <param name="firewall"> Firerwall object</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddFirewall(string key, IFirewall firewall)
        {
            _logger.LogInformation("Persisting firewall object with key: " + key);
            _dbMock.TryAdd(key, firewall);
            DoBackup();
        }

        /// <summary>
        ///Method allows to get the Firewall object from in memory database for the specific OVS.
        /// </summary>
        /// <param name="key">Open Virtual Switch prefix</param>
        /// <returns>Firewall Object</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IFirewall GetFirewall(string key)
        {
            _dbMock.TryGetValue(key, out IFirewall value);
            return value;
        }

        /// <summary>
        /// Method allows to remove the Firewall object from in memory database for the specific OVS.
        /// </summary>
        /// <param name="key">Open Virtual Switch prefix</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveFirewall(string key)
        {
            _dbMock.TryRemove(key, out _);
            DoBackup();
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        private void DoBackup()
        {
            File.WriteAllText(_backupPath, JsonConvert.SerializeObject(_dbMock, _jsonSerializerSettings));
        }
    }
}
