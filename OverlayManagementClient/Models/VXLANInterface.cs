namespace OverlayManagementClient.Services
{
    public class VXLANInterface
    {
        private string Name { get; set; }
        private Bridge Parent { get; set; }
        private string Type { get; set; }
        private string RemoteIp { get; set; }
        private string Key { get; set; }
        private string OpenFlowPort { get; set; }
        private string SourcePort { get; set; }
    }
}