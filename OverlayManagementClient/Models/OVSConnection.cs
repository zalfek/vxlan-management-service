
using System.ComponentModel.DataAnnotations;

namespace OverlayManagementClient.Models
{
    public class OVSConnection
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string GroupId { get; set; }
        public VmConnection VmConnection { get; set; } = null;
    }
}
