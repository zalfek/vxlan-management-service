using System;

namespace OverlayManagementService.Network
{
    /// <summary>
    /// Class which encapsulates functionality needed for deployment of the VXLAN on Target device.
    /// </summary>
    public class TargetDevice : ITargetDevice
    {

        public TargetDevice(Guid guid, string username, string key, string managementIp, string vni, string destIP, string communicationIP)
        {
            Key = key;
            Username = username;
            Guid = guid;
            ManagementIp = managementIp;
            Vni = vni;
            DestIp = destIP;
            CommunicationIP = communicationIP;
            VXLANInterface = new LinuxVXLANInterface(Key + "-vxlan" + Vni, Vni, "4789", DestIp);
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

        /// <summary>
        /// Method triggers cleanup of the VXLAN interface on target device.
        /// </summary>
        public void CleanUpVMConnection()
        {
            VXLANInterface.CleanUpInterface(Username, Guid.ToString(), ManagementIp);
        }

        /// <summary>
        /// Method triggers deployment of the VXLAN interface on target device.
        /// </summary>
        /// <param name="vxlanIp">ip address which was assigned to the target device</param>
        public void DeployVMConnection(string vxlanIp)
        {
            VxlanIp = vxlanIp;
            VXLANInterface.DeployInterface(Username, Guid.ToString(), ManagementIp, vxlanIp);
        }
    }
}
