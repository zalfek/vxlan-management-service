using OverlayManagementService.Dtos;
using System.Collections.Generic;

namespace OverlayManagementService.Network
{
    public interface IOpenVirtualSwitch
    {
        public string Key { get; set; }
        public void AddBridge(IBridge bridge);
        public void DeployVXLANInterface(ITargetDevice virtualMachine);
        public void DeployClientVXLANInterface(string vni, string ip);
        public void CleanUpClientVXLANInterface(string vni, string ip);
        public IDictionary<string, IBridge> Bridges { get; set; }
        public void DeployBridge(string vni);
        public void CleanUpBridge(string vni);
        public string PrivateIP { get; set; }
        public string PublicIP { get; set; }
        public string ManagementIp { get; set; }
        public void RemoveTargetConnection(ITargetDevice virtualMachine);
        void DeployVXLANInterface(ExternalSwitchEndpoint externalSwitchEndpoint);
    }
}
