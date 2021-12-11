using System;

namespace OverlayConnectionClient.Models
{
    public class OverlayNetwork
    {
        public string VNI { get; set; }
        public Guid Guid { get; set; }
        public string RemoteIp { get; set; }
    }
}