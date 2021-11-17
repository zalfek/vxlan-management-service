using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public interface IOpenVirtualSwitch
    {
        public void CreateConnection(IVirtualMachine virtualMachine);
        public void RemoveConnection(IVirtualMachine virtualMachine);

    }
}
