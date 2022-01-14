namespace OverlayConnectionClient.Network
{
    public interface ILinuxVXLANInterface
    {
        public void DeployInterface();
        public void CleanUpInterface();
        public string Name { get; set; }
        public string VNI { get; set; }
        public string DstPort { get; set; }
        public string DstIP { get; set; }
        public string LocalIP { get; set; }

    }
}
