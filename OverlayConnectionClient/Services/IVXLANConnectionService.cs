using OverlayConnectionClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementClient.Services
{
    public interface IVXLANConnectionService
    {
        public Task<IEnumerable<OverlayNetwork>> GetNetworksAsync();
        public Task<OverlayNetwork> GetNetworkAsync(string id);
    }
}
