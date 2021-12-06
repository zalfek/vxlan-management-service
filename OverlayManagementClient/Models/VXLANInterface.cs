namespace OverlayManagementClient.Models
{
    public class VXLANInterface
    {
        public string Name { get; set; }
        private Bridge Parent { get; set; }
        public string Type { get; set; }
        public string RemoteIp { get; set; }
        public string Key { get; set; }
        public string OpenFlowPort { get; set; }
        public string SourcePort { get; set; }
    }
}