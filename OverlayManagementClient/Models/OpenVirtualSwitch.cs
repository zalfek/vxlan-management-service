using Microsoft.AspNetCore.Http;
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


        public OpenVirtualSwitch(string key, string managementIp, string privateIP, string publicIp)
        {
            Key = key;
            PrivateIP = privateIP;
            PublicIP = publicIp;
            Bridges = new Dictionary<string, Bridge>();
            ManagementIp = managementIp;
        }

        public OpenVirtualSwitch()
        {
            Bridges = new Dictionary<string, Bridge>();
        }

    }
}