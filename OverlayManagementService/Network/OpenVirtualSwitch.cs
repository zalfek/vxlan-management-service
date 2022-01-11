using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace OverlayManagementService.Network
{

    /// <summary>
    /// Class which encapsulates functionality needed for deployment of the VXLAN on Open Virtual Switch.
    /// </summary>
    public class OpenVirtualSwitch : IOpenVirtualSwitch
    {

        private readonly ILogger<IOpenVirtualSwitch> _logger;

        public IDictionary<string, IBridge> Bridges { get; set; }
        public string PrivateIP { get; set; }
        public string PublicIP { get; set; }
        public string Key { get; set; }
        public string ManagementIp { get; set; }

        public OpenVirtualSwitch(string key, string managementIp, string privateIP, string publicIp)
        {
            _logger = LoggerFactory.Create(logging => logging.AddConsole()).CreateLogger<IOpenVirtualSwitch>();
            Key = key;
            PrivateIP = privateIP;
            PublicIP = publicIp;
            Bridges = new Dictionary<string, IBridge>();
            ManagementIp = managementIp;
    }

        /// <summary>
        /// Method allows to add a new Bridge to a list of bridges in OVS.
        /// </summary>
        /// <param name="bridge">Bridge object</param>
        public void AddBridge(IBridge bridge) {
            Bridges.Add(bridge.Vni, bridge);
        }

        /// <summary>
        /// Method triggers deployment of the VXLAN interface to OVS bridge.
        /// </summary>
        /// <param name="virtualMachine">IVIrtualMachine(target device) object </param>
        public void DeployVXLANInterface(ITargetDevice virtualMachine)
        {
            _logger.LogInformation("Initiating vxlan interface deployment on Bridge with VNI: " + virtualMachine.Vni + " with remote ip: " + virtualMachine.CommunicationIP);
            Bridges[virtualMachine.Vni].DeployVXLANInterface(virtualMachine.CommunicationIP);
        }

        /// <summary>
        /// Method triggers deployment of the client VXLAN interface to OVS bridge.
        /// </summary>
        /// <param name="vni">VNI of the network to which interface should be deployed</param>
        /// <param name="ip">IP address of the client</param>
        public void DeployClientVXLANInterface(string vni, string ip)
        {
            _logger.LogInformation("Initiating vxlan interface deployment on Bridge with VNI: " + vni + " with client(remote) ip: " + ip);
            Bridges[vni].DeployClientVXLANInterface(ip);
        }

        /// <summary>
        /// Method triggers cleanup of the client VXLAN interface to OVS bridge.
        /// </summary>
        /// <param name="vni">VNI of the network where interface was deployed</param>
        /// <param name="ip">IP address of the client</param>
        public void CleanUpClientVXLANInterface(string vni, string ip)
        {
            _logger.LogInformation("Initiating vxlan interface clean up on Bridge with VNI: " + vni + " with client(remote) ip: " + ip);
            Bridges[vni].CleanUpClientVXLANInterface(ip);
        }

        /// <summary>
        /// Method triggers Bridge deployment on OVS.
        /// </summary>
        /// <param name="vni">VNI of the network where bridge should be deployed</param>
        public void DeployBridge(string vni)
        {
            _logger.LogInformation("Deploying new Bridge with VNI: " + vni);
            Bridges[vni].DeployBridge();
        }

        /// <summary>
        /// Method deploys the VXLAN interface to OVS bridge.
        /// </summary>
        /// <param name="vni">VNI of the network where bridge was deployed</param>
        public void CleanUpBridge(string vni)
        {
            foreach (KeyValuePair<string, IBridge> entry in Bridges)
            {
                if (entry.Value.Vni == vni)
                {
                    entry.Value.CleanUpBridge();
                    Bridges.Remove(entry.Key);
                }
            }
        }

        /// <summary>
        /// Method triggers cleanup of the VXLAN interface(Target) from OVS bridge.
        /// </summary>
        /// <param name="targetDevice">TargetDevice object</param>
        public void RemoveTargetConnection(ITargetDevice targetDevice)
        {
            Bridges[targetDevice.Vni].CleanUpTargetVXLANInterface(targetDevice.CommunicationIP);
        }
    }
}
