namespace OverlayManagementService.Network
{
    public interface IVXLANInterface
    {
        public string RemoteIp { get; set; }
        public void DeployVXLANInterface(string username, string key, string managementIp);
        public void CleanUpVXLANInterface(string username, string key, string managementIp);
    }
}
