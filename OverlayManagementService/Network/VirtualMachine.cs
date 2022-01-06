using OverlayManagementService.Dtos;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OverlayManagementService.Network
{
    public class VirtualMachine : IVirtualMachine
    {

        public VirtualMachine(Guid guid, string username, string key, string managementIp, string vni, string destIP, string communicationIP)
        {
            Key = key;
            Username = username;
            Guid = guid;
            ManagementIp = managementIp;
            Vni = vni;
            DestIp = destIP;
            CommunicationIP = communicationIP;
            VXLANInterface = new LinuxVXLANInterface(Username , guid.ToString(),  Key + "-vxlan" + Vni, Vni, "4789", DestIp, managementIp);
        }

        public Guid Guid { get; set; }
        public string Vni { get; set; }
        public string ManagementIp { get; set; }
        public string DestIp { get; set; }
        public string VxlanIp { get; set; }
        public ILinuxVXLANInterface VXLANInterface;
        public string CommunicationIP { get; set; }
        public string Key { get; set; }
        public string Username { get; set; }

        public void CleanUpVMConnection()
        {
            VXLANInterface.CleanUpInterface();
        }

        public void DeployVMConnection(string vxlanIp)
        {
            VxlanIp = vxlanIp;
            VXLANInterface.DeployInterface(vxlanIp);
        }
    }
}
