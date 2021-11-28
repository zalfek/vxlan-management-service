using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public interface IVirtualMachine
    {
        public string IPAddress { get; set; }
        public string VNI { get; set; }
        public void DeployVMConnection();
        public void CleanUpVMConnection();
    }
}
