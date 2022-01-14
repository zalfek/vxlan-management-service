using OverlayManagementService.Dtos;
using OverlayManagementService.Network;
using System.Collections.Generic;

namespace OverlayManagementService.Services
{
    public interface IOverlayNetworkManagementService
    {
        public IOverlayNetwork DeployNetwork(OVSConnection oVSConnection);
        public void DeleteNetwork(string groupId);
        public IEnumerable<IOverlayNetwork> GetAllNetworks();
        public IOverlayNetwork GetNetworkByVni(string vni);
        public IOverlayNetwork UpdateNetwork(IOverlayNetwork overlayNetwork);
    }
}
