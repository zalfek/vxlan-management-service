using OverlayManagementService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Factories
{
    public class ClientConnectionFactory : IClientConnectionFactory
    {
        public ClientConnection CreateClientConnectionDto(string vNI, string groupId, string remoteIp, string localIp)
        {
            return new ClientConnection(vNI, groupId, remoteIp, localIp);
        }
    }
}
