using OverlayManagementService.Dtos;
using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Services
{
    public interface IOverlayManagementService
    {
        public IOpenVirtualSwitch AddSwitch(IOpenVirtualSwitch openVirtualSwitch);
        public IOverlayNetwork DeployNetwork(OVSConnection oVSConnection);
        public void DeleteNetwork(string groupId);
        public IOverlayNetwork RegisterMachine(VmConnection vmConnection);
        public IOverlayNetwork UnRegisterMachine(string groupId, Guid guid);
        public IEnumerable<IOverlayNetwork> GetAllNetworks();
        public IOverlayNetwork GetNetworkByVni(string vni);
        public IEnumerable<IOpenVirtualSwitch> GetAllSwitches();
        public IOpenVirtualSwitch GetSwitch(string key);
        public IOverlayNetwork UpdateNetwork(IOverlayNetwork overlayNetwork);
        public void RemoveSwitch(string key);
    }
}
