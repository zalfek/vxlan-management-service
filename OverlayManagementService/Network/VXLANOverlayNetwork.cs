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
            IpAddress = ipAddress;
        }

        public string Vni { get; set; }
        public string GroupId { get; set; }
        public IOpenVirtualSwitch OpenVirtualSwitch { get; set; }
        public List<IVirtualMachine> VirtualMachines { get; set; }
        public List<string> Clients { get; set; }
        public bool IsDeployed { get; set; }
        public IAddress IpAddress { get; set; }

        public string AddClient(string ip)
        {
            Clients.Add(ip);
            OpenVirtualSwitch.DeployClientVXLANInterface(Vni, ip);
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

        public void RemoveClient(string ip)
        {
            Clients.Remove(ip);
            OpenVirtualSwitch.CleanUpClientVXLANInterface(Vni, ip);
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
