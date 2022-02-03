using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Dtos
{
    public class ExternalSwitchEndpoint
    {
        public string CommunicationIP { get; set; }
        public string GroupId { get; set; }
        public string Vni { get; set; }
    }
}
