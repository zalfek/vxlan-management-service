using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Factories
{
    public class VirtualMachineFactory : IVirtualMachineFactory
    {
        public IVirtualMachine CreateVirtualMachine(Guid guid, string username, string key, string managementIp, string vni, string destIP, string communicationIP)
        {
            return new VirtualMachine(guid, username, key, managementIp, vni, destIP, communicationIP);
        }
    }
}
