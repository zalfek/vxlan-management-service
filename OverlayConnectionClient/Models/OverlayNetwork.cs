using System;

namespace OverlayConnectionClient.Models
{
    public class OverlayNetwork
    {
        public string VNI { get; set; }
        public string GroupId { get; set; }
        public string RemoteIp { get; set; }
        public string LocalIp { get; set; }
    }
}