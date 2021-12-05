using System.Collections.Generic;

namespace OverlayManagementClient.Models
{
    public class OpenVirtualSwitch
    {
        public IDictionary<string, Bridge> Bridges { get; set; }
        public string PrivateIP { get; set; }
        public string PublicIP { get; set; }
        public string Key { get; set; }
        public string ManagementIp { get; set; }
    }
}