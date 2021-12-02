using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Factories
{
    public class NetworkFactory : INetworkFactory
    {
        public IOverlayNetwork CreateOverlayNetwork(string vni, IOpenVirtualSwitch openVirtualSwitch)
        {
            return new VXLANOverlayNetwork(vni, openVirtualSwitch);
        }
    }
}
