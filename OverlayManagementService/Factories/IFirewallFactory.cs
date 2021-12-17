using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Factories
{
    public interface IFirewallFactory
    {
        public IFirewall CreateFirewall(string managementIp);
    }
}
