using System;

namespace OverlayManagementClient.Models
{
    public class TargetDevice
    {
        public Guid Guid { get; set; }
        public string VNI { get; set; }
        public string ManagementIp { get; set; }
        public string DestIp { get; set; }
        public string VxlanIp { get; set; }
        public LinuxVXLANInterface VXLANInterface;
        public string CommunicationIP { get; set; }
    }
}