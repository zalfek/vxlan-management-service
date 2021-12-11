using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayConnectionClient.Network
{
    public interface ILinuxVXLANInterface
    {
        public void DeployInterface();
        public void CleanUpInterface();


    }
}
