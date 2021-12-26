using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OverlayManagementClient.Models
{
    public class VmConnection
    {
        [Required]
        public string ManagementIp { get; set; }
        [Required]
        public string CommunicationIP { get; set; }
        [Required]
        public string GroupId { get; set; }
        [Required]
        public IFormFile KeyFile { get; set; }
    }
}