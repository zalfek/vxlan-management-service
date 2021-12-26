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
    public class SwitchRepository : ISwitchRepository
    {
        private readonly ILogger<ISwitchRepository> _logger;
        private static IDictionary<string, IOpenVirtualSwitch> _dbMock;
        private readonly string _backupPath;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        public SwitchRepository()
        {
            _logger = new LoggerFactory().CreateLogger<ISwitchRepository>();
            _backupPath = Path.Combine(AppContext.BaseDirectory, "Resources", "SwitchData.json");
            _jsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            _dbMock =JsonConvert.DeserializeObject<Dictionary<string, IOpenVirtualSwitch>>(File.ReadAllText(_backupPath), _jsonSerializerSettings);
        }

        public void DeleteSwitch(string key)
        {
            _dbMock.Remove(key);
            File.WriteAllText(_backupPath, JsonConvert.SerializeObject(_dbMock, _jsonSerializerSettings));
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
            File.WriteAllText(_backupPath, JsonConvert.SerializeObject(_dbMock, _jsonSerializerSettings));
            return openVirtualSwitch;
        }

        public IOpenVirtualSwitch UpdateSwitch(IOpenVirtualSwitch openVirtualSwitch)
        {
            _dbMock[openVirtualSwitch.Key] = openVirtualSwitch;
            File.WriteAllText(_backupPath, JsonConvert.SerializeObject(_dbMock, _jsonSerializerSettings));
            return openVirtualSwitch;
        }

        public IDictionary<string, IOpenVirtualSwitch> GetAllSwitches()
        {
            return _dbMock;
        }

    }
}
