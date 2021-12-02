using OverlayManagementService.Dtos;
using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Services
{
    public interface IOverlayConnectionService
    {
        public IOverlayNetwork GetOverlayNetwork(Membership membership);

    }
}
