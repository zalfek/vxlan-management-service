using OverlayConnectionClient.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayConnectionClient.Factories
{
    public class LinuxVxlanInterfaceFactory : ILinuxVxlanInterfaceFactory
    {
        public ILinuxVXLANInterface CreateInterface(string vni, string remoteIp, string localIP)
        {
            return new LinuxVXLANInterface("vxlan"+ vni, vni, "4789",  remoteIp, localIP);
        } 
    }
}
