using OverlayManagementService.Network;

namespace OverlayManagementService.Factories
{
    public interface IFirewallFactory
    {
        public IFirewall CreateFirewall(string managementIp);
    }
}
