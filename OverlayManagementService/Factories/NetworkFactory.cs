using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Factories
{
    public class NetworkFactory : INetworkFactory
    {
        public IOverlayNetwork CreateOverlayNetwork(string groupId, string vni, IOpenVirtualSwitch openVirtualSwitch, IAddress ipAddress)
        {
            return new VXLANOverlayNetwork(groupId, vni, openVirtualSwitch, ipAddress);
        }
    }
}
