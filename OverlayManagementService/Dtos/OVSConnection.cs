using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Dtos
{
    public class OVSConnection
    {
        public string Key { get; set; }
        public string MembershipId { get; set; }
        public VmConnection vmConnection { get; set; } = null;
    }
}
