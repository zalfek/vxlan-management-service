using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Factories
{
    public class BridgeFactory : IBridgeFactory
    {
        public IBridge CreateBridge(string username, string key, string name, string vni, string managementIp)
        {
            return new Bridge(username, key, name, vni, managementIp);
        }
    }
}
