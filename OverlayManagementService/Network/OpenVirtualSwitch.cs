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
            _logger = new LoggerFactory().CreateLogger<OpenVirtualSwitch>();
            Key = key;
            PrivateIP = privateIP;
            PublicIP = publicIp;
            Bridges = new Dictionary<string, IBridge>();
            ManagementIp = managementIp;
        }


        public void AddBridge(IBridge bridge) {
            Bridges.Add(bridge.Vni, bridge);
        }


        public void DeployVXLANInterface(IVirtualMachine virtualMachine)
        {
            _logger.LogInformation("Initiating vxlan interface deployment on Bridge with VNI: " + virtualMachine.Vni + "with remote ip: " + virtualMachine.CommunicationIP);
            Bridges[virtualMachine.Vni].DeployVXLANInterface(virtualMachine);
        }

        public void DeployClientVXLANInterface(string vni, string ip)
        {
            _logger.LogInformation("Initiating vxlan interface deployment on Bridge with VNI: " + vni + "with client(remote) ip: " + ip);
            Bridges[vni].DeployClientVXLANInterface(ip);
        }

        public void CleanUpClientVXLANInterface(string vni, string ip)
        {
            _logger.LogInformation("Initiating vxlan interface clean up on Bridge with VNI: " + vni + "with client(remote) ip: " + ip);
            Bridges[vni].CleanUpClientVXLANInterface(ip);
        }

        public void DeployOVSConnection(string vni)
        {
            _logger.LogInformation("Deploying new Bridge with VNI: " + vni);
            Bridges[vni].DeployBridge();
        }

        public void CleanUpOVSConnection(IVirtualMachine virtualMachine)
        {
            foreach (KeyValuePair<string, IBridge> entry in Bridges)
            {
                if (entry.Value.Vni == virtualMachine.Vni)
                {
                    entry.Value.CleanUpBridge();
                    Bridges.Remove(entry.Key);
                }
            }
        }
    }
}
