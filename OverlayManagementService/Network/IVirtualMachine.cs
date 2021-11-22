using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public interface IVirtualMachine
    {
        string IPAddress { get; set; }

        public void DeployVMConnection(string ipAddress, IOverlayNetwork overlayNetwork);
        public void CleanUpVMConnection();
    }
}
