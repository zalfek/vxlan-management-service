using OverlayManagementService.DataTransferObjects;
using OverlayManagementService.Network;
using System;
using System.Collections.Generic;

namespace OverlayManagementService.Network
{
    public class VXLANOverlayNetwork : IOverlayNetwork
    {
        public VXLANOverlayNetwork()
        {
        }

        public VXLANOverlayNetwork(string vNI, Guid guid, List<IOpenVirtualSwitch> openVirtualSwitch, List<IVirtualMachine> virtualMachine, List<IUser> user, bool isDeployed)
        {
            VNI = vNI;
            Guid = guid;
            OpenVirtualSwitch = openVirtualSwitch;
            VirtualMachine = virtualMachine;
            User = user;
            IsDeployed = isDeployed;
        }

        private string VNI { get; set; }
        private Guid Guid { get; set; }
        private List<IOpenVirtualSwitch> OpenVirtualSwitch { get; set; }
        private List<IVirtualMachine> VirtualMachine { get; set; }
        private List<IUser> User { get; set; }
        private bool IsDeployed { get; set; }

        public void CleanUpNetwork()
        {
            throw new NotImplementedException();
        }

        public void DeployNetwork()
        {
            throw new NotImplementedException();
        }
    }
}
