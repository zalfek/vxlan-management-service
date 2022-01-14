using OverlayManagementService.Network;

namespace OverlayManagementService.Repositories
{
    public interface IFirewallRepository
    {
        public void AddFirewall(string key, IFirewall firewall);
        public IFirewall GetFirewall(string key);
        public void RemoveFirewall(string key);
    }
}
