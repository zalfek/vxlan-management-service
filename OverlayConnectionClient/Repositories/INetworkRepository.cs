using OverlayConnectionClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayConnectionClient.Repositories
{
    public interface INetworkRepository
    {
        public Task<IEnumerable<OverlayNetwork>> GetNetworksAsync();
        public Task<OverlayNetwork> GetNetworkAsync(string groupId);
        public void RemoveClientAsync(string groupId);
    }
}
