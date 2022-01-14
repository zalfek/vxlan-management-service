using OverlayManagementService.Network;

namespace OverlayManagementService.Factories
{
    public class FirewallFactory : IFirewallFactory
    {
        public IFirewall CreateFirewall(string managementIp) { return new Firewall(managementIp); }
    }
}
