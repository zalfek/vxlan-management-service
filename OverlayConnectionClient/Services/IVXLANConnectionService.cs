using OverlayConnectionClient.Models;
using System.Collections.Generic;

namespace OverlayConnectionClient.Services
{
    public interface IVXLANConnectionService
    {
        public IEnumerable<OverlayNetwork> GetAllNetworks();
        public void CreateConnection(string groupId);
        public void CleanUpConnection(string groupId);
    }
}
