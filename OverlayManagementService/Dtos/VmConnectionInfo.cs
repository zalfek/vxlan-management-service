using OverlayManagementService.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Dtos
{
    public class VmConnectionInfo : IVmConnectionInfo
    {

        public string IPAddress { get; set; }
        public string Membership { get; set; }

        public VmConnectionInfo(string ipAddress, string membership)
        {
            IPAddress = ipAddress;
            Membership = membership;
        }
    }
}
