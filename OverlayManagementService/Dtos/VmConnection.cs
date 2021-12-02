using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Dtos
{
    public class VmConnection
    {
        public string Key { get; set; }
        public string ManagementIp { get; set; }
        public string CommunicationIP { get; set; }
        public string OVSIPAddress { get; set; }
        public string Membership { get; set; }

    }
}
