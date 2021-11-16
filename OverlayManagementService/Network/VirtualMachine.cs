using System;

namespace OverlayManagementService.Network
{
    public class VirtualMachine : IVirtualMachine
    {
        public Guid Id { get; set; }
        public string IPAddress { get; set; }

    }
}
