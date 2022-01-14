namespace OverlayManagementService.Network
{
    public interface IBridge
    {
        public string Vni { get; set; }
        string Name { get; }
        public void DeployVXLANInterface(string ip);
        public void DeployClientVXLANInterface(string ip);
        public void CleanUpClientVXLANInterface(string ip);
        public void DeployBridge();
        public void CleanUpBridge();
        public void CleanUpTargetVXLANInterface(string ip);
    }
}
