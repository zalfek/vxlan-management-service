using OverlayManagementService.Dtos;
using OverlayManagementService.Models;
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
        public List<Student> Clients { get; set; }
        public IOpenVirtualSwitch OpenVirtualSwitch { get; set; }
        public void RemoveClient(Student client);
        public string AddClient(Student client);
        public void RemoveVMachine(Guid guid);
        public void AddVMachine(IVirtualMachine virtualMachine);
        public void DeployNetwork();
        public void CleanUpNetwork();
    }
}
