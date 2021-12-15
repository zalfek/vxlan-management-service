using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OverlayConnectionClient.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OverlayConnectionClient.Repositories
{
    public class JsonRepository : IRepository
    {

        private readonly ILogger<IRepository> _logger;
        private static IDictionary<string, ILinuxVXLANInterface> _dbMock;
        public JsonRepository()
        {
            _logger = new LoggerFactory().CreateLogger<IRepository>();
            _dbMock = new Dictionary<string, ILinuxVXLANInterface>();
        }

        public void DeleteInterface(string groupId)
        {
            _dbMock.Remove(groupId);
        }

        public ILinuxVXLANInterface GetVXLANInterface(string groupId)
        {
            return _dbMock[groupId];
        }

        public ILinuxVXLANInterface SaveInterface(string groupId, ILinuxVXLANInterface linuxVXLANInterface)
        {
            if (_dbMock.ContainsKey(groupId))
            {
                _dbMock[groupId] = linuxVXLANInterface;
            }
            else
            {
                _dbMock.Add(groupId, linuxVXLANInterface);
            }
            return linuxVXLANInterface;
        }

    }
}
