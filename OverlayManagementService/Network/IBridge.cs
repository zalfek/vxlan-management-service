using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public interface IBridge
    {
        string Name { get; }
        IVeth VirtualInterface { get; }
        public void DeployVXLANInterface(IVirtualMachine virtualMachine);
        public void DeployBridge();
        public void CleanUpBridge();
    }
}
