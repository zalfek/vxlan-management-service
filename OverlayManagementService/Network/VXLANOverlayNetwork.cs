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

        public VXLANOverlayNetwork(string vNI, List<IOpenVirtualSwitch> openVirtualSwitches, List<IVirtualMachine> virtualMachines, List<IUser> clients)
        {
            VNI = vNI;
            Guid = Guid.NewGuid();
            OpenVirtualSwitches = openVirtualSwitches;
            VirtualMachines = virtualMachines;
            Clients = clients;
            IsDeployed = false;
        }

        public string VNI { get; set; }
        public Guid Guid { get; set; }
        public List<IOpenVirtualSwitch> OpenVirtualSwitches { get; set; }
        public List<IVirtualMachine> VirtualMachines { get; set; }
        public List<IUser> Clients { get; set; }
        public bool IsDeployed { get; set; }

        public void AddClient(IUser user)
        {
            Clients.Add(user);
        }

        public void AddSwitch(IOpenVirtualSwitch openVirtualSwitch)
        {
            OpenVirtualSwitches.Add(openVirtualSwitch);
        }

        public void AddVMachine(IVirtualMachine virtualMachine)
        {
            OpenVirtualSwitches[0].DeployVXLANInterface(virtualMachine);
            VirtualMachines.Add(virtualMachine);
        }

        public void CleanUpNetwork()
        {
            VirtualMachines.ForEach(vm => OpenVirtualSwitches.ForEach(sw => { 
                vm.CleanUpVMConnection();
                sw.CleanUpOVSConnection(vm);
            }));
        }

        public void DeployNetwork()
        {
            VirtualMachines.ForEach(vm => OpenVirtualSwitches[0].DeployOVSConnection(vm));
            IsDeployed = true;
        }

        public void RemoveClient(IUser user)
        {
            Clients.Remove(user);
        }

        public void RemoveSwitch(IOpenVirtualSwitch openVirtualSwitch)
        {
            VirtualMachines.ForEach(vm => { 
                openVirtualSwitch.CleanUpOVSConnection(vm);
                vm.CleanUpVMConnection();
            });
            OpenVirtualSwitches.Remove(openVirtualSwitch);
        }

        public void RemoveVMachine(IVirtualMachine virtualMachine)
        {
            OpenVirtualSwitches.ForEach(sw => { 
                sw.CleanUpOVSConnection(virtualMachine);
                virtualMachine.CleanUpVMConnection();
            });
            VirtualMachines.Remove(virtualMachine);
        }
    }
}
