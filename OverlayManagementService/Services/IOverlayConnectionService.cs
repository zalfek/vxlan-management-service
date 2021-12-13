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
        public ClientConnection CreateConnection(string groupId, string ip);
        public IEnumerable<ClientConnection> GetAllNetworks(IEnumerable<Claim> claims);
    }
}
