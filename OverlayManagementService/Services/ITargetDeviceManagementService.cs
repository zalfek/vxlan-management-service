using OverlayManagementService.Dtos;
using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Services
{
    public interface ITargetDeviceManagementService
    {
        public IOverlayNetwork RegisterMachine(VmConnection vmConnection);
        public IOverlayNetwork UnRegisterMachine(string groupId, Guid guid);
    }
}
