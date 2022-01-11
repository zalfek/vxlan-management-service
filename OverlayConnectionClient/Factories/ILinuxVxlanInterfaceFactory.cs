using OverlayConnectionClient.Network;


namespace OverlayConnectionClient.Factories
{
    public interface ILinuxVxlanInterfaceFactory
    {
        public ILinuxVXLANInterface CreateInterface(string remoteIp, string vni, string localIP);
    }
}
