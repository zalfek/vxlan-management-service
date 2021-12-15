using OverlayManagementService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public interface IOverlayNetwork
    {
        string GroupId { get; set; }
        string Vni { get; set; }
        public IOpenVirtualSwitch OpenVirtualSwitch { get; set; }
        public void RemoveClient(string ip);
        public void AddClient(string ip);
        public void RemoveVMachine(IVirtualMachine virtualMachine);
        public void AddVMachine(IVirtualMachine virtualMachine);
        public void DeployNetwork();
        public void CleanUpNetwork();

    }
}
