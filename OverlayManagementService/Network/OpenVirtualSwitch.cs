using Microsoft.Extensions.Logging;
using OverlayManagementService.Network;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public class OpenVirtualSwitch : IOpenVirtualSwitch
    {

        private readonly ILogger<OpenVirtualSwitch> _logger;

        public IDictionary<string, IBridge> Bridges { get; set; }
        public string PrivateIP { get; set; }
        public string PublicIP { get; set; }
        public string Key { get; set; }
        public string ManagementIp { get; set; }


        public OpenVirtualSwitch(string key, string managementIp, string privateIP, string publicIp)
        {
            Key = key;
            PrivateIP = privateIP;
            PublicIP = publicIp;
            Bridges = new Dictionary<string, IBridge>();
            ManagementIp = managementIp;
        }


        public void AddBridge(IBridge bridge) {
            Bridges.Add(bridge.VNI, bridge);
        }


        public void DeployVXLANInterface(IVirtualMachine virtualMachine)
        {
            Bridges[virtualMachine.VNI].DeployVXLANInterface(virtualMachine);
        }

        public void DeployClientVXLANInterface(string vni, string ip)
        {
            Bridges[vni].DeployClientVXLANInterface(ip);
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
