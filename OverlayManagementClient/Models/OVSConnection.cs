
namespace OverlayManagementClient.Models
{
    public class OVSConnection
    {
        public string Key { get; set; }
        public string GroupId { get; set; }
        public VmConnection VmConnection { get; set; } = null;
    }
}
