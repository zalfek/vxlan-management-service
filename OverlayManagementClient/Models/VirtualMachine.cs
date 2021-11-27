using System;
using System.Collections.Generic;

namespace OverlayManagementClient.Services
{
    public class VirtualMachine
    {
        private Guid Guid { get; set; }
        public string IPAddress { get; set; }
        private VmConnectionInfo VmConnectionInfo { get; set; }
        public List<LinuxVXLANInterface> VXLANInterfaces;
    }
}