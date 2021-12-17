using OverlayManagementService.Dtos;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OverlayManagementService.Network
{
    public class VirtualMachine : IVirtualMachine
    {

        public VirtualMachine(Guid guid, string managementIp, string vni, string destIP, string communicationIP)
        {
            Guid = guid;
            ManagementIp = managementIp;
            Vni = vni;
            DestIp = destIP;
            CommunicationIP = communicationIP;
            VXLANInterface = new LinuxVXLANInterface(Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 14), Vni, "4789", DestIp, managementIp);
        }

        public Guid Guid { get; set; }
        public string Vni { get; set; }
        public string ManagementIp { get; set; }
        public string DestIp { get; set; }
        public string VxlanIp { get; set; }
        public ILinuxVXLANInterface VXLANInterface;
        public string CommunicationIP { get; set; }

        public void CleanUpVMConnection()
        {
            VXLANInterface.CleanUpInterface();
        }

        public void DeployVMConnection(string vxlanIp)
        {
            VXLANInterface.DeployInterface(vxlanIp);
        }
    }
}
