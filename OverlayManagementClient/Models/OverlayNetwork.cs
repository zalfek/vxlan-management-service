using System;
using System.Collections.Generic;

namespace OverlayManagementClient.Models
{
    public class OverlayNetwork
    {
        public string VNI { get; set; }
        public Guid Guid { get; set; }
        public List<OpenVirtualSwitch> OpenVirtualSwitches { get; set; }
        public List<VirtualMachine> VirtualMachines { get; set; }
        public List<User> Clients { get; set; }
        public bool IsDeployed { get; set; }
    }
}