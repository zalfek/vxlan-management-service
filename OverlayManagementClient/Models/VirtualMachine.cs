using System;
using System.Collections.Generic;

namespace OverlayManagementClient.Models
{
    public class VirtualMachine
    {
        private Guid Guid { get; set; }
        public string IPAddress { get; set; }
        private VmConnection VmConnectionInfo { get; set; }
        public List<LinuxVXLANInterface> VXLANInterfaces;
    }
}