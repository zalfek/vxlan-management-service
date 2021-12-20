using Microsoft.Extensions.Logging;
using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Repositories
{
    public class FirewallRepository : IFirewallRepository
    {
        private readonly ILogger<IFirewallRepository> _logger;
        private static IDictionary<string, IFirewall> _dbMock;
        public FirewallRepository()
        {
            _logger = new LoggerFactory().CreateLogger<IFirewallRepository>();
            _dbMock = new Dictionary<string, IFirewall>();
        }

        public void AddFirewall(string key, IFirewall firewall)
        {
            _logger.LogInformation("Persisting firewall object with key: " + key);
            _dbMock.Add(key, firewall);
        }
        public IFirewall GetFirewall(string key)
        {
            return _dbMock[key];
        }

        public void RemoveFirewall(string key)
        {
            _dbMock.Remove(key);
        }
    }
}
