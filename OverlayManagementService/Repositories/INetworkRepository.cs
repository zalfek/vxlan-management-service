using OverlayManagementService.Network;
using System.Collections.Generic;
using System.Security.Claims;

namespace OverlayManagementService.Repositories
{
    public interface INetworkRepository
    {
        IOverlayNetwork SaveOverlayNetwork(IOverlayNetwork overlayNetwork);
        void DeleteOverlayNetwork(string groupId);
        IOverlayNetwork UpdateOverlayNetwork(IOverlayNetwork overlayNetwork);
        IOverlayNetwork GetOverlayNetwork(string groupId);
        IDictionary<string, IOverlayNetwork> GetAllNetworks();
        IOverlayNetwork GetOverlayNetworkByVni(string vni);
        IOverlayNetwork GetOverlayNetwork(Claim claim);
    }
}
