using System;

namespace OverlayManagementService.Network
{
    public interface ITargetDevice
    {
        public string ManagementIp { get; set; }
        public string Vni { get; set; }
        public string CommunicationIP { get; set; }
        public Guid Guid { get; set; }
        public void DeployVMConnection(string vxlanIp);
        public void CleanUpVMConnection();
    }
}
