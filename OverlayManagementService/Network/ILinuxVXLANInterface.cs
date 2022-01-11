using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public interface ILinuxVXLANInterface
    {
        public void DeployInterface(string username, string key, string managementIp, string vxlanIp);
        public void CleanUpInterface(string username, string key, string managementIp);


    }
}
