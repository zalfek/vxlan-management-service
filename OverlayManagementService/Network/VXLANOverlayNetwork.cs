using OverlayManagementService.Dtos;
using OverlayManagementService.Models;
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
            Clients = new List<Student>();
            IsDeployed = false;
            IpAddress = ipAddress;
        }

        public string Vni { get; set; }
        public string GroupId { get; set; }
        public IOpenVirtualSwitch OpenVirtualSwitch { get; set; }
        public List<IVirtualMachine> VirtualMachines { get; set; }
        public List<Student> Clients { get; set; }
        public bool IsDeployed { get; set; }
        public IAddress IpAddress { get; set; }

        public string AddClient(Student client)
        {
            Clients.Add(client);
            OpenVirtualSwitch.DeployClientVXLANInterface(Vni, client.IpAddress);
            return IpAddress.GenerarteUniqueIPV4Address();
        }

        public void AddVMachine(IVirtualMachine virtualMachine)
        {
            virtualMachine.DeployVMConnection(IpAddress.GenerarteUniqueIPV4Address());
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

        public void RemoveClient(Student client)
        {
            Clients.Remove(client);
            OpenVirtualSwitch.CleanUpClientVXLANInterface(Vni, client.IpAddress);
        }

        public void RemoveVMachine(Guid guid)
        {
            IVirtualMachine virtualMachine = VirtualMachines.Find(x => x.Guid == guid);
            OpenVirtualSwitch.RemoveVMConnection(virtualMachine);
            virtualMachine.CleanUpVMConnection();
            VirtualMachines.Remove(virtualMachine);
        }

    }
}
