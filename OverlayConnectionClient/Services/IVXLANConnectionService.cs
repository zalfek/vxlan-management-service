using OverlayConnectionClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayConnectionClient.Services
{
    public interface IVXLANConnectionService
    {
        public IEnumerable<OverlayNetwork> GetAllNetworks();
        public void CreateConnection(string groupId);
        public void CleanUpConnection(string groupId);
    }
}
