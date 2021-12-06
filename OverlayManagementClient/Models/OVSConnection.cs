
namespace OverlayManagementClient.Models
{
    public class OVSConnection
    {
        public string Key { get; set; }
        public string MembershipId { get; set; }
        public VmConnection vmConnection { get; set; } = null;
    }
}
