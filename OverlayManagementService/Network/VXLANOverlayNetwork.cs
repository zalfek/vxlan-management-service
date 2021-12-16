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

        public VXLANOverlayNetwork(string groupId, string vNI, IOpenVirtualSwitch openVirtualSwitch, IAddress ipAddress)
        {
            Vni = vNI;
            GroupId = groupId;
            OpenVirtualSwitch = openVirtualSwitch;
            VirtualMachines = new List<IVirtualMachine>();
            Clients = new List<string>();
            IsDeployed = false;
        }

        public string Vni { get; set; }
        public string GroupId { get; set; }
        public IOpenVirtualSwitch OpenVirtualSwitch { get; set; }
        public List<IVirtualMachine> VirtualMachines { get; set; }
        public List<string> Clients { get; set; }
        public bool IsDeployed { get; set; }
        public IAddress ipAddress { get; set; }

        public void AddClient(string ip)
        {
            Clients.Add(ip);
            OpenVirtualSwitch.DeployClientVXLANInterface(Vni, ip);
        }

        public void AddVMachine(IVirtualMachine virtualMachine)
        {
            virtualMachine.DeployVMConnection(ipAddress.GenerarteUniqueIPV4Address());
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
            OpenVirtualSwitch.DeployOVSConnection(Vni);
            IsDeployed = true;
        }

        public void RemoveClient(string ip)
        {
            Clients.Remove(ip);
        }

        public void RemoveVMachine(Guid guid)
        {
            IVirtualMachine virtualMachine = VirtualMachines.Find(x => x.Guid == guid);
            OpenVirtualSwitch.CleanUpOVSConnection(virtualMachine);
            virtualMachine.CleanUpVMConnection();
            VirtualMachines.Remove(virtualMachine);
        }
    }
}
