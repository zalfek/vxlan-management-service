using OverlayManagementService.Dtos;
using Renci.SshNet;
using System;
using System.Collections.Generic;

namespace OverlayManagementService.Network
{
    public class VirtualMachine : IVirtualMachine
    {

        public VirtualMachine(Guid guid, string ipAddress, string vni, string vxlanIp, string destIP)
        {
            SSHConnectionInfo = new ConnectionInfo(ipAddress, "vagrant", new AuthenticationMethod[]{
                            new PasswordAuthenticationMethod("vagrant", "vagrant")
            });
            Guid = guid;
            IPAddress = ipAddress;
            VNI = vni;
            VxlanIp = vxlanIp;
            DestIp = destIP;
            VXLANInterface = new LinuxVXLANInterface(Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 14), VNI, "4789", DestIp, VxlanIp, SSHConnectionInfo);
        }

        private Guid Guid { get; set; }
        public string VNI { get; set; }
        public string IPAddress { get; set; }
        public string DestIp { get; set; }
        public string VxlanIp { get; set; }
        public ILinuxVXLANInterface VXLANInterface;
        private ConnectionInfo SSHConnectionInfo;

        public void CleanUpVMConnection()
        {
            VXLANInterface.CleanUpInterface();
        }

        public void DeployVMConnection()
        {
            VXLANInterface.DeployInterface();
        }
    }
}
