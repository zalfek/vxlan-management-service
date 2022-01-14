using System.Collections.Generic;

namespace OverlayManagementClient.Models
{
    public class OverlayNetwork
    {
        public string VNI { get; set; }
        public string GroupId { get; set; }
        public OpenVirtualSwitch OpenVirtualSwitch { get; set; }
        public List<TargetDevice> TargetDevices { get; set; }
        public List<Student> Clients { get; set; }
    }
}