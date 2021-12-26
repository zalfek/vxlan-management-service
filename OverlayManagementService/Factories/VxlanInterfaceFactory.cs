using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Factories
{
    public class VxlanInterfaceFactory : IVxlanInterfaceFactory
    {
        public IVXLANInterface CreateInterface(string username, string key, string name, string type, string remoteIp, string vni, string bridgeName, string managementIp)
        {
            return new VXLANInterface(username, key, name, type, remoteIp, vni, bridgeName, managementIp);
        } 
    }
}
