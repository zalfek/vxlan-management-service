using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public interface IOpenVirtualSwitch
    {
        public string Key { get; set; }
        public void AddBridge(IBridge bridge);
        public void DeployVXLANInterface(IVirtualMachine virtualMachine);
        public void DeployClientVXLANInterface(string vni, string ip);
        public IDictionary<string, IBridge> Bridges { get; set; }
        public void DeployOVSConnection(string vni);
        public void CleanUpOVSConnection(IVirtualMachine virtualMachine);
        public string PrivateIP { get; set; }
        public string PublicIP { get; set; }
        public string ManagementIp { get; set; }
    }
}
