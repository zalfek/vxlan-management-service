using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public interface ILinuxVXLANInterface
    {
        public void DeployInterface(string vxlanIp);
        public void CleanUpInterface();


    }
}
