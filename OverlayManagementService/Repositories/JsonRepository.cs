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

        public void DeleteOverlayNetwork(string membership)
        {
            _dbMock.Remove(membership);
        }

        public IOverlayNetwork GetOverlayNetwork(string membership)
        {
            return _dbMock[membership];
        }

        public IOverlayNetwork SaveOverlayNetwork(string membership, IOverlayNetwork overlayNetwork)
        {
            if (_dbMock.ContainsKey(membership))
            {
                _dbMock[membership] = overlayNetwork;
            }
            else
            {
                _dbMock.Add(membership, overlayNetwork);
            }
            return overlayNetwork;
        }

        public IOverlayNetwork UpdateOverlayNetwork(string membership, IOverlayNetwork overlayNetwork)
        {
            _dbMock[membership] = overlayNetwork;
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

        public IOverlayNetwork GetOverlayNetworkByVni(string id)
        {
            foreach (KeyValuePair<string, IOverlayNetwork> keyValuePair in _dbMock)
            {
                if (keyValuePair.Value.VNI == id) { return keyValuePair.Value; }
            }
            throw new KeyNotFoundException();
        }

        public IOverlayNetwork GetOverlayNetwork(Claim claim)
        {
            return _dbMock[claim.Value];
        }
    }
}
