using OverlayManagementService.Dtos;
using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Services
{
    public interface IOverlayManagementService
    {
        public IOverlayNetwork RegisterMachine(IVmConnectionInfo vmConnectionInfo);
        public IOverlayNetwork UnRegisterMachine(IVmConnectionInfo vmConnectionInfo);

    }
}
