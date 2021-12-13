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

        public VXLANOverlayNetwork(string vNI, IOpenVirtualSwitch openVirtualSwitch, List<IVirtualMachine> virtualMachines, List<string> clients)
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
            Clients = new List<string>();
            IsDeployed = false;
        }

        public string VNI { get; set; }
        public Guid Guid { get; set; }
        public IOpenVirtualSwitch OpenVirtualSwitch { get; set; }
        public List<IVirtualMachine> VirtualMachines { get; set; }
        public List<string> Clients { get; set; }
        public bool IsDeployed { get; set; }

        public void AddClient(string ip)
        {
            Clients.Add(ip);
            OpenVirtualSwitch.DeployClientVXLANInterface(VNI, ip);
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

        public void RemoveClient(string ip)
        {
            Clients.Remove(ip);
        }

        public void RemoveVMachine(IVirtualMachine virtualMachine)
        {
            OpenVirtualSwitch.CleanUpOVSConnection(virtualMachine);
            virtualMachine.CleanUpVMConnection();
            VirtualMachines.Remove(virtualMachine);
        }
    }
}
