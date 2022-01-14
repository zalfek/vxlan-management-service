using OverlayManagementService.Dtos;

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
