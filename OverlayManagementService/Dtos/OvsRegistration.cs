using Microsoft.AspNetCore.Http;

namespace OverlayManagementService.Dtos
{
    public class OvsRegistration
    {
        public string PrivateIP { get; set; }
        public string PublicIP { get; set; }
        public string Key { get; set; }
        public string ManagementIp { get; set; }
        public IFormFile KeyFile { get; set; }

    }
}
