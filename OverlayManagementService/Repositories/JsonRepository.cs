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
    public class JsonRepository : IRepository
    {

        private readonly ILogger<IRepository> _logger;
        private static IDictionary<string, IOverlayNetwork> _dbMock;
        public JsonRepository()
        {
            _logger = new LoggerFactory().CreateLogger<IRepository>();
            _dbMock = new Dictionary<string, IOverlayNetwork>();
        }

        public void DeleteOverlayNetwork(string groupId)
        {
            _dbMock.Remove(groupId);
        }

        public IOverlayNetwork GetOverlayNetwork(string groupId)
        {
            return _dbMock[groupId];
        }

        public IOverlayNetwork SaveOverlayNetwork(string groupId, IOverlayNetwork overlayNetwork)
        {
            if (_dbMock.ContainsKey(groupId))
            {
                _dbMock[groupId] = overlayNetwork;
            }
            else
            {
                _dbMock.Add(groupId, overlayNetwork);
            }
            return overlayNetwork;
        }

        public IOverlayNetwork UpdateOverlayNetwork(string groupId, IOverlayNetwork overlayNetwork)
        {
            _dbMock[groupId] = overlayNetwork;
            return overlayNetwork;
        }

        public IDictionary<string, IOverlayNetwork> GetAllNetworks()
        {
            return _dbMock;
        }

        public IEnumerable<IOverlayNetwork> GetAllSwitches()
        {
            throw new NotImplementedException();
        }

        public IOverlayNetwork GetOverlayNetworkByVni(string vni)
        {
            foreach (KeyValuePair<string, IOverlayNetwork> keyValuePair in _dbMock)
            {
                if (keyValuePair.Value.VNI == vni) { return keyValuePair.Value; }
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
