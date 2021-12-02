using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Factories
{
    public interface IBridgeFactory
    {
        public IBridge CreateBridge(string name, string vni, string managementIp);
    }
}
