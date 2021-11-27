using OverlayManagementService.Dtos;
using Renci.SshNet;
using System;
using System.Collections.Generic;

namespace OverlayManagementService.Network
{
    public class VirtualMachine : IVirtualMachine
    {

        public VirtualMachine(Guid guid, IVmConnectionInfo vmConnectionInfo)
        {
            Guid = guid;
            IPAddress = vmConnectionInfo.IPAddress;
            VmConnectionInfo = vmConnectionInfo;
            VXLANInterfaces = new List<ILinuxVXLANInterface>();
        }

        private Guid Guid { get; set; }
        public string IPAddress { get; set; }
        private IVmConnectionInfo VmConnectionInfo { get; set; }
        public List<ILinuxVXLANInterface> VXLANInterfaces;

        public void CleanUpVMConnection()
        {
            VXLANInterfaces.ForEach(infs =>
            {
                infs.CleanUpInterface();
            });
        }

        public void DeployVMConnection(string ipAddress, IOverlayNetwork overlayNetwork)
        {
            ILinuxVXLANInterface linuxVXLANInterface = new LinuxVXLANInterface(Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 14),overlayNetwork.VNI,"4789", overlayNetwork.OpenVirtualSwitches[0].Bridges[0].VirtualInterface.IpAddress, ipAddress);
            linuxVXLANInterface.DeployInterface();
            VXLANInterfaces.Add(linuxVXLANInterface);
        }
    }
}
