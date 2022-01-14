using OverlayConnectionClient.Models;
using System.Collections.Generic;
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
