using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Factories
{
    public class VxlanInterfaceFactory : IVxlanInterfaceFactory
    {
        public IVXLANInterface CreateInterface(string remoteIp, string vni, string bridgeName)
        {
            return new VXLANInterface( Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 14), "vxlan", remoteIp, vni, bridgeName);
        } 
    }
}
