using Microsoft.Extensions.Logging;
using OverlayManagementService.Network;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public class OpenVirtualSwitch : IOpenVirtualSwitch
    {

        private readonly ILogger<OpenVirtualSwitch> _logger;
        public ConnectionInfo SSHConnectionInfo { get; set; }
        public IDictionary<string, IBridge> Bridges { get; set; }
        public string PrivateIP { get; set; }
        public string PublicIP { get; set; }
        public string Key { get; set; }



        public OpenVirtualSwitch(ConnectionInfo sSHConnectionInfo)
        {
            SSHConnectionInfo = sSHConnectionInfo;
            Bridges = new Dictionary<string, IBridge>();
        }


        public void AddBridge(IBridge bridge) {
            Bridges.Add(bridge.VNI, bridge);
        }


        public void DeployVXLANInterface(IVirtualMachine virtualMachine)
        {
            Bridges[virtualMachine.VNI].DeployVXLANInterface(virtualMachine);
        }

        public void DeployOVSConnection(string vni)
        {
            Bridges[vni].DeployBridge();
        }

        public void CleanUpOVSConnection(IVirtualMachine virtualMachine)
        {
            foreach (KeyValuePair<string, IBridge> entry in Bridges)
            {
                entry.Value.CleanUpBridge();
                Bridges.Remove(entry.Key);
            }
        }
    }
}
