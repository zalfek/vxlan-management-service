using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Dtos
{
    public interface IVmConnectionInfo

    {
        string IPAddress { get; set; }
        string Membership { get; set; }
    }
}
