using OverlayManagementService.Network;

namespace OverlayManagementService.Factories
{
    public interface IBridgeFactory
    {
        public IBridge CreateBridge(string username, string key, string name, string vni, string managementIp);
    }
}
