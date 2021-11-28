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
        public IOpenVirtualSwitch AddSwitch(string key);
        public IOverlayNetwork DeployNetwork(OVSConnection oVSConnection);
        public IOverlayNetwork SuspendNetwork(Membership membership);
        public void DeleteNetwork(Membership membership);
        public IOverlayNetwork RegisterMachine(VmConnection vmConnection);
        public IOverlayNetwork UnRegisterMachine(VmConnection vmConnection);

    }
}
