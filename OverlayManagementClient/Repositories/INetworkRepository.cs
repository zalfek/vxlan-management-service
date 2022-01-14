using OverlayManagementClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OverlayManagementClient.Repositories
{
    public interface INetworkRepository
    {
        public Task<OverlayNetwork> AddNetworkAsync(OVSConnection oVSConnection);
        public Task DeleteNetworkAsync(string groupId);
        public Task<OverlayNetwork> EditNetworkAsync(OverlayNetwork OverlayNetwork);
        public Task<IEnumerable<OverlayNetwork>> GetNetworksAsync();
        public Task<OverlayNetwork> GetNetworkAsync(string id);
    }
}
