using OverlayManagementService.Dtos;
using OverlayManagementService.Models;
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
        public ClientConnection CreateConnection(string groupId, Student client);
        public IEnumerable<ClientConnection> GetAllNetworks(IEnumerable<Claim> claims);
    }
}
