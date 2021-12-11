using OverlayManagementService.Dtos;
using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OverlayManagementService.Services
{
    public interface IOverlayConnectionService
    {
        public IOverlayNetwork CreateConnection(string membership, string ip);
        public IEnumerable<IOverlayNetwork> GetAllNetworks(IEnumerable<Claim> claims);
    }
}
