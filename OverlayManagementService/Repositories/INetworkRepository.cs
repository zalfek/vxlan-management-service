using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OverlayManagementService.Repositories
{
    public interface INetworkRepository
    {
        IOverlayNetwork SaveOverlayNetwork(IOverlayNetwork overlayNetwork);
        void DeleteOverlayNetwork(string groupId);
        IOverlayNetwork UpdateOverlayNetwork(IOverlayNetwork overlayNetwork);
        IOverlayNetwork GetOverlayNetwork(string groupId);
        IDictionary<string, IOverlayNetwork> GetAllNetworks();
        IEnumerable<IOverlayNetwork> GetAllSwitches();
        IOverlayNetwork GetOverlayNetworkByVni(string vni);
        IOverlayNetwork GetOverlayNetwork(Claim claim);
    }
}
