using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Repositories
{
    public class FirewallRepository : IFirewallRepository
    {
        private readonly ILogger<IFirewallRepository> _logger;
        private static IDictionary<string, IFirewall> _dbMock;
        private readonly string _backupPath;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        public FirewallRepository()
        {
            _logger = new LoggerFactory().CreateLogger<IFirewallRepository>();
            _backupPath = Path.Combine(AppContext.BaseDirectory, "Resources", "FirewallData.json");
            _jsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            _dbMock = JsonConvert.DeserializeObject<Dictionary<string, IFirewall>>(File.ReadAllText(_backupPath), _jsonSerializerSettings);
        }

        public void AddFirewall(string key, IFirewall firewall)
        {
            _logger.LogInformation("Persisting firewall object with key: " + key);
            _dbMock.Add(key, firewall);
            File.WriteAllText(_backupPath, JsonConvert.SerializeObject(_dbMock, _jsonSerializerSettings));
        }
        public IFirewall GetFirewall(string key)
        {
            return _dbMock[key];
        }

        public void RemoveFirewall(string key)
        {
            _dbMock.Remove(key);
            File.WriteAllText(_backupPath, JsonConvert.SerializeObject(_dbMock));
        }
    }
}
