using OverlayManagementService.Network;

namespace OverlayManagementService.Factories
{
    public interface IVxlanInterfaceFactory
    {
        public IVXLANInterface CreateInterface(string remoteIp, string vni, string bridgeName);
    }
}
