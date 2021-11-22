using OverlayManagementService.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public interface IOverlayNetwork
    {
        string VNI { get; set; }
        List<IOpenVirtualSwitch> OpenVirtualSwitches { get; set; }

        public void RemoveClient(IUser user);
        public void AddClient(IUser user);
        public void RemoveVMachine(IVirtualMachine virtualMachine);
        public void AddVMachine(IVirtualMachine virtualMachine);
        public void RemoveSwitch(IOpenVirtualSwitch openVirtualSwitch);
        public void AddSwitch(IOpenVirtualSwitch openVirtualSwitch);
        public void DeployNetwork();
        public void CleanUpNetwork();

    }
}
