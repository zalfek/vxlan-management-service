using OverlayManagementService.Dtos;
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

        public VXLANOverlayNetwork(string vNI, IOpenVirtualSwitch openVirtualSwitch, List<IVirtualMachine> virtualMachines, List<Student> clients)
        {
            VNI = vNI;
            Guid = Guid.NewGuid();
            OpenVirtualSwitch = openVirtualSwitch;
            VirtualMachines = virtualMachines;
            Clients = clients;
            IsDeployed = false;
        }

        public VXLANOverlayNetwork(string vNI, IOpenVirtualSwitch openVirtualSwitch)
        {
            VNI = vNI;
            Guid = Guid.NewGuid();
            OpenVirtualSwitch = openVirtualSwitch;
            VirtualMachines = new List<IVirtualMachine>();
            Clients = new List<Student>();
            IsDeployed = false;
        }

        public string VNI { get; set; }
        public Guid Guid { get; set; }
        public IOpenVirtualSwitch OpenVirtualSwitch { get; set; }
        public List<IVirtualMachine> VirtualMachines { get; set; }
        public List<Student> Clients { get; set; }
        public bool IsDeployed { get; set; }

        public void AddClient(Student user)
        {
            Clients.Add(user);
        }

        public void AddSwitch(IOpenVirtualSwitch openVirtualSwitch)
        {
            
        }

        public void AddVMachine(IVirtualMachine virtualMachine)
        {
            OpenVirtualSwitch.DeployVXLANInterface(virtualMachine);
            VirtualMachines.Add(virtualMachine);
        }

        public void CleanUpNetwork()
        {
            VirtualMachines.ForEach(vm => { 
                vm.CleanUpVMConnection();
                OpenVirtualSwitch.CleanUpOVSConnection(vm);
            });
        }

        public void DeployNetwork()
        {
            OpenVirtualSwitch.DeployOVSConnection(VNI);
            IsDeployed = true;
        }

        public void RemoveClient(Student user)
        {
            Clients.Remove(user);
        }

        public void RemoveSwitch(IOpenVirtualSwitch openVirtualSwitch)
        {
            VirtualMachines.ForEach(vm => { 
                openVirtualSwitch.CleanUpOVSConnection(vm);
                vm.CleanUpVMConnection();
            });
        }

        public void RemoveVMachine(IVirtualMachine virtualMachine)
        {
            OpenVirtualSwitch.CleanUpOVSConnection(virtualMachine);
            virtualMachine.CleanUpVMConnection();
            VirtualMachines.Remove(virtualMachine);
        }
    }
}
