using OverlayConnectionClient.Network;

namespace OverlayConnectionClient.Factories
{
    public class LinuxVxlanInterfaceFactory : ILinuxVxlanInterfaceFactory
    {
        public ILinuxVXLANInterface CreateInterface(string vni, string remoteIp, string localIP)
        {
            return new LinuxVXLANInterface("vxlan" + vni, vni, "4789", remoteIp, localIP);
        }
    }
}
