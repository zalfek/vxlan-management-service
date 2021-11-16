using OverlayManagementService.DataTransferObjects;
using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Repositories
{
    public interface IRepository
    {
        IOverlayNetwork GetOverlayNetwork(Membership membership);
    }
}
