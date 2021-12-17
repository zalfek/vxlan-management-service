using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public interface IVXLANInterface
    {
        public string RemoteIp { get; set; }
        public void DeployVXLANInterface();
        public void CleanUpVXLANInterface();
    }
}
