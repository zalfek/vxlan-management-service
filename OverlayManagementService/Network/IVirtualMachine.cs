using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public interface IVirtualMachine
    {
        public string ManagementIp { get; set; }
        public string VNI { get; set; }
        public string CommunicationIP { get; set; }

        public void DeployVMConnection(string vxlanIp);
        public void CleanUpVMConnection();
    }
}
