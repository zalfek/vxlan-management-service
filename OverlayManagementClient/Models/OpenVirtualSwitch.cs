using System.Collections.Generic;

namespace OverlayManagementClient.Services
{
    public class OpenVirtualSwitch
    {
        public List<Bridge> Bridges { get; set; }
        public string PrivateIP;
        public string PublicIP;
    }
}