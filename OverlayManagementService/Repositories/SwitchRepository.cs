using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OverlayManagementService.Network;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace OverlayManagementService.Repositories
{

    /// <summary>
    /// Class encapsulates the logic reguired to persist Switch objects.
    /// </summary>
    public class SwitchRepository : ISwitchRepository
    {
        private readonly ILogger<ISwitchRepository> _logger;
        private static ConcurrentDictionary<string, IOpenVirtualSwitch> _dbMock;
        private readonly string _backupPath;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        public SwitchRepository()
        {
            _logger = new LoggerFactory().CreateLogger<ISwitchRepository>();
            _backupPath = Path.Combine(AppContext.BaseDirectory, "Resources", "SwitchData.json");
            _jsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            _dbMock = JsonConvert.DeserializeObject<ConcurrentDictionary<string, IOpenVirtualSwitch>>(File.ReadAllText(_backupPath), _jsonSerializerSettings);
        }

        /// <summary>
        /// Method allows to remove the Switch object from in memory database for the specific switch prefix.
        /// </summary>
        /// <param name="key">Open Virtual Switch prefix</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteSwitch(string key)
        {
            _logger.LogInformation("Deleting Switch with Key:" + key + " from in-memory db");
            _dbMock.TryRemove(key, out _);
            _logger.LogInformation("Creating in-memory database backup");
            DoBackup();
        }

        /// <summary>
        /// Method allows to get the Switch object from in memory database for the specific switch prefix.
        /// </summary>
        /// <param name="key">Open Virtual Switch prefix</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IOpenVirtualSwitch GetSwitch(string key)
        {
            _dbMock.TryGetValue(key, out IOpenVirtualSwitch value);
            return value;
        }

        /// <summary>
        /// Method allows to save the Switch object to in memory database with the specific prefix.
        /// </summary>
        /// <param name="openVirtualSwitch">OpenVirtualSwitch object</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IOpenVirtualSwitch SaveSwitch(IOpenVirtualSwitch openVirtualSwitch)
        {
            if (_dbMock.ContainsKey(openVirtualSwitch.Key))
            {
                _logger.LogInformation("Updating existing Switch with Key:" + openVirtualSwitch.Key + " in in-memory db");
                _dbMock.TryGetValue(openVirtualSwitch.Key, out IOpenVirtualSwitch value);
                _dbMock.TryUpdate(openVirtualSwitch.Key, openVirtualSwitch, value);
            }
            else
            {
                _logger.LogInformation("Adding Switch with Key:" + openVirtualSwitch.Key + " to in-memory db");
                _dbMock.TryAdd(openVirtualSwitch.Key, openVirtualSwitch);
            }
            _logger.LogInformation("Creating in-memory database backup");
            DoBackup();
            return openVirtualSwitch;
        }

        /// <summary>
        /// Method allows to update exiing Switch object in in-memory database.
        /// </summary>
        /// <param name="openVirtualSwitch">OpenVirtualSwitch object</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IOpenVirtualSwitch UpdateSwitch(IOpenVirtualSwitch openVirtualSwitch)
        {
            _logger.LogInformation("Updating existing Switch with Key:" + openVirtualSwitch.Key + " in in-memory db");
            _dbMock.TryGetValue(openVirtualSwitch.Key, out IOpenVirtualSwitch value);
            _dbMock.TryUpdate(openVirtualSwitch.Key, openVirtualSwitch, value);
            _logger.LogInformation("Creating in-memory database backup");
            DoBackup();
            return openVirtualSwitch;
        }

        /// <summary>
        /// Method allows to get all deployed switch objects from in memory database.
        /// </summary>
        /// <returns>Dictionary with maping Azure AD group to OpenVirtualSwitch</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IDictionary<string, IOpenVirtualSwitch> GetAllSwitches()
        {
            return _dbMock;
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        private void DoBackup()
        {
            File.WriteAllText(_backupPath, JsonConvert.SerializeObject(_dbMock, _jsonSerializerSettings));
        }

    }
}
