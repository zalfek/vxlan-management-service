using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Factories
{
    public interface INetworkFactory
    {
        public IOverlayNetwork CreateOverlayNetwork(string vni, IOpenVirtualSwitch openVirtualSwitch);
    }
}
