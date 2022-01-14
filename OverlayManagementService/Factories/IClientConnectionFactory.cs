using OverlayManagementService.Dtos;

namespace OverlayManagementService.Factories
{
    public interface IClientConnectionFactory
    {
        public ClientConnection CreateClientConnectionDto(string vNI, string groupId, string remoteIp, string localIp);
    }
}
