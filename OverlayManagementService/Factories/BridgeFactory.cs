using OverlayManagementService.Network;

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
