using OverlayManagementService.Dtos;
using OverlayManagementService.Network;
using System;

namespace OverlayManagementService.Services
{
    public interface ITargetDeviceManagementService
    {
        public IOverlayNetwork RegisterMachine(VmConnection vmConnection);
        public IOverlayNetwork UnRegisterMachine(string groupId, Guid guid);
    }
}
