using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public interface IBridge
    {
        public string Vni { get; set; }
        string Name { get; }
        public void DeployVXLANInterface(IVirtualMachine virtualMachine);
        public void DeployClientVXLANInterface(string ip);
        public void CleanUpClientVXLANInterface(string ip);
        public void DeployBridge();
        public void CleanUpBridge();
        public void CleanUpTargetVXLANInterface(string ip);
    }
}
