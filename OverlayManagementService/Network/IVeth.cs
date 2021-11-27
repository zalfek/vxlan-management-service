using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public interface IVeth
    {
        string IpAddress { get; set; }
        public void DeployVeth();
        public void CleanUpVeth();
    }
}
