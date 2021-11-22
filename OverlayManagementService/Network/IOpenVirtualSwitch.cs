using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public interface IOpenVirtualSwitch
    {
        public void AddBridge(IBridge bridge);
        public void DeployVXLANInterface(IVirtualMachine virtualMachine);
        List<IBridge> Bridges { get; set; }
        public void DeployOVSConnection(IVirtualMachine virtualMachine);
        public void CleanUpOVSConnection(IVirtualMachine virtualMachine);
    }
}
