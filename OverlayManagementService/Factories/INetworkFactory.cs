using OverlayManagementService.Network;

namespace OverlayManagementService.Factories
{
    public interface INetworkFactory
    {
        public IOverlayNetwork CreateOverlayNetwork(string groupId, string vni, IOpenVirtualSwitch openVirtualSwitch, IAddress ipAddress);
    }
}
