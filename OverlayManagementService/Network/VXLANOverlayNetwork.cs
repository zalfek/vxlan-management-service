using OverlayManagementService.Network;
using System;

namespace OverlayManagementService.Network
{
    public class VXLANOverlayNetwork : IOverlayNetwork
    {
        public string DestIpAddress { get; set; }
        public string Port { get; set; }
        public string VNI { get; set; }
        public Guid Guid { get; set; }
    }
}
