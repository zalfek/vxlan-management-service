using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public interface IBridge
    {
        public void DeployBridge();
        public void CleanUpBridge();
    }
}
