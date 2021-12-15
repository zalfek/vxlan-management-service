using OverlayManagementService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Factories
{
    public interface IClientConnectionFactory
    {
        public ClientConnection CreateClientConnectionDto(string vNI, string groupId, string remoteIp, string localIp);
    }
}
