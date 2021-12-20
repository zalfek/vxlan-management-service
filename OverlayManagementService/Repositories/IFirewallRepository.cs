using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Repositories
{
    public interface IFirewallRepository
    {
        public void AddFirewall(string key, IFirewall firewall);
        public IFirewall GetFirewall(string key);
        public void RemoveFirewall(string key);
    }
}
