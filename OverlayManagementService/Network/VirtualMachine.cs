using OverlayManagementService.Dtos;
using System;

namespace OverlayManagementService.Network
{
    public class VirtualMachine : IVirtualMachine
    {
        public VirtualMachine(Guid guid, string iPAddress, VmConnectionInfo vmConnectionInfo)
        {
            Guid = guid;
            IPAddress = iPAddress;
            VmConnectionInfo = vmConnectionInfo;
        }

        private Guid Guid { get; set; }
        private string IPAddress { get; set; }
        private VmConnectionInfo VmConnectionInfo { get; set; }

        public void CleanUpVMConnection(IOpenVirtualSwitch openVirtualSwitch)
        {
            throw new NotImplementedException();
        }

        public void DeployVMConnection(IOpenVirtualSwitch openVirtualSwitch)
        {
            throw new NotImplementedException();
        }
    }
}
