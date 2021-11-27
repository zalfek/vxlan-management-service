using OverlayManagementService.DataTransferObjects;
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
        public IOverlayNetwork DeployNetwork(IVmConnectionInfo vmConnectionInfo);
        public IOverlayNetwork SuspendNetwork(IMembership membership);
        public void DeleteNetwork(IMembership membership);
        public IOverlayNetwork RegisterMachine(IVmConnectionInfo vmConnectionInfo);
        public IOverlayNetwork UnRegisterMachine(IVmConnectionInfo vmConnectionInfo);

    }
}
