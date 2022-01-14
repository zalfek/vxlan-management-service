using OverlayManagementService.Dtos;
using OverlayManagementService.Network;

namespace OverlayManagementService.Factories
{
    public interface IOpenVirtualSwitchFactory
    {
        public IOpenVirtualSwitch CreateSwitch(OvsRegistration ovsRegistration);
    }
}
