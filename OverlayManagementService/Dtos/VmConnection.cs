using Microsoft.AspNetCore.Http;

namespace OverlayManagementService.Dtos
{
    public class VmConnection
    {
        public string ManagementIp { get; set; }
        public string CommunicationIP { get; set; }
        public string GroupId { get; set; }
        public IFormFile KeyFile { get; set; }

    }
}
