using System;

namespace OverlayManagementService.Network
{
    public class VirtualMachine : IVirtualMachine
    {
        public Guid Id { get; set; }
        public string IPAddress { get; set; }

        public void CreateConnection(IOpenVirtualSwitch openVirtualSwitch)
        {
            throw new NotImplementedException();
        }

        public void RemoveConnection(IOpenVirtualSwitch openVirtualSwitch)
        {
            throw new NotImplementedException();
        }
    }
}
