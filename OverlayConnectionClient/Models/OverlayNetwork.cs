using System;
using System.Net.NetworkInformation;

namespace OverlayConnectionClient.Models
{
    public class OverlayNetwork
    {
        public string VNI { get; set; }
        public string GroupId { get; set; }
        public string RemoteIp { get; set; }
        public string LocalIp { get; set; }



        public bool IsConnected()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                if(adapter.Name == "vxlan" + VNI) { return true; }
            }
                return false;
        }

    }
}