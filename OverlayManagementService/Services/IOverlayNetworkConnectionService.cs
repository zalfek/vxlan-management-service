using OverlayManagementService.Dtos;
using OverlayManagementService.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace OverlayManagementService.Services
{
    public interface IOverlayNetworkConnectionService
    {
        public ClientConnection CreateConnection(string groupId, Student client);
        public IEnumerable<ClientConnection> GetAllNetworks(IEnumerable<Claim> claims);
        public void SuspendConnection(string groupId, Student client);
    }
}
