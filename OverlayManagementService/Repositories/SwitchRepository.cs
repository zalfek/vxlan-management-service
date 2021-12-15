using Microsoft.Extensions.Logging;
using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Repositories
{
    public class SwitchRepository : ISwitchRepository
    {
        private readonly ILogger<ISwitchRepository> _logger;
        private static IDictionary<string, IOpenVirtualSwitch> _dbMock;
        public SwitchRepository()
        {
            _logger = new LoggerFactory().CreateLogger<ISwitchRepository>();
            _dbMock = new Dictionary<string, IOpenVirtualSwitch>();
        }

        public void DeleteSwitch(string key)
        {
            _dbMock.Remove(key);
        }

        public IOpenVirtualSwitch GetSwitch(string key)
        {
            return _dbMock[key];
        }

        public IOpenVirtualSwitch SaveSwitch(IOpenVirtualSwitch openVirtualSwitch)
        {
            if (_dbMock.ContainsKey(openVirtualSwitch.Key))
            {
                _dbMock[openVirtualSwitch.Key] = openVirtualSwitch;
            }
            else
            {
                _dbMock.Add(openVirtualSwitch.Key, openVirtualSwitch);
            }
            return openVirtualSwitch;
        }

        public IOpenVirtualSwitch UpdateSwitch(IOpenVirtualSwitch openVirtualSwitch)
        {
            _dbMock[openVirtualSwitch.Key] = openVirtualSwitch;
            return openVirtualSwitch;
        }

        public IDictionary<string, IOpenVirtualSwitch> GetAllSwitches()
        {
            return _dbMock;
        }

    }
}
