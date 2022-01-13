using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace OverlayManagementClient.Models
{
    public class OvsRegistration
    {
        [Required]
        public string PrivateIP { get; set; }
        [Required]
        public string PublicIP { get; set; }
        [Required]
        public string Key { get; set; }
        [Required]
        public string ManagementIp { get; set; }
        [Required]
        public IFormFile KeyFile { get; set; }

    }
}
