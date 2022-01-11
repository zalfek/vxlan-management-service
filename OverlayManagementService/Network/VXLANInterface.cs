using Microsoft.Extensions.Logging;
using OverlayManagementService.Services;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{

    /// <summary>
    /// Class which encapsulates VXLAN interface deployment and cleanup functionality on Open Virtual Switch.
    /// </summary>
    public class VXLANInterface : IVXLANInterface                                                                                                   
    {
        public string Name { get; set; }
        public string BridgeName { get; set; }
        public string Type { get; set; }
        public string RemoteIp { get; set; }
        public string Vni { get; set; }
        private readonly ILogger<IVXLANInterface> _logger;

        public VXLANInterface(string name, string type, string remoteIp, string vni, string bridgeName)
        {

            _logger = LoggerFactory.Create(logging => logging.AddConsole()).CreateLogger<IVXLANInterface>();
            Name = name;
            Type = type;
            RemoteIp = remoteIp;
            Vni = vni;
            BridgeName = bridgeName;
        }

        /// <summary>
        /// Method deletes the VXLAN interface from OVS bridge.
        /// </summary>
        /// <param name="username">username to access the OVS</param>
        /// <param name="key">Switch prefix which was used for naming the private key for this OVS</param>
        /// <param name="managementIp">Management ip ov the OVS</param>
        public void CleanUpVXLANInterface(string username, string key, string managementIp)
        {
            ConnectionInfo sSHConnectionInfo = new(managementIp, username, new AuthenticationMethod[]{
             new PrivateKeyAuthenticationMethod(username, new PrivateKeyFile[]{
                    new PrivateKeyFile(KeyKeeper.GetInstance().GetKeyLocation(key))
                }),
            });

            using var sshclient = new SshClient(sSHConnectionInfo);
            _logger.LogInformation("Connecting to device");
            sshclient.Connect();
            using (var cmd = sshclient.CreateCommand("sudo ovs-vsctl del-port " + BridgeName + " " + Name))
            {
                _logger.LogInformation("Executing command: " + cmd.CommandText);
                cmd.Execute();
                _logger.LogInformation("Command>" + cmd.CommandText);
                _logger.LogInformation("Return Value = {0}", cmd.ExitStatus);
            }
            _logger.LogInformation("Disconnecting from device");
            sshclient.Disconnect();
        }

        /// <summary>
        /// Method deploys the VXLAN interface to OVS bridge.
        /// </summary>
        /// <param name="username">username to access the OVS</param>
        /// <param name="key">Switch prefix which was used for naming the private key for this OVS</param>
        /// <param name="managementIp">Management ip ov the OVS</param>
        public void DeployVXLANInterface(string username, string key, string managementIp)
        {
            ConnectionInfo sSHConnectionInfo = new(managementIp, username, new AuthenticationMethod[]{
             new PrivateKeyAuthenticationMethod(username, new PrivateKeyFile[]{
                    new PrivateKeyFile(KeyKeeper.GetInstance().GetKeyLocation(key))
                }),
            });

            using var sshclient = new SshClient(sSHConnectionInfo);
            _logger.LogInformation("Connecting to device");
            sshclient.Connect();
            using (var cmd = sshclient.CreateCommand("sudo ovs-vsctl add-port " + BridgeName + " " + Name + " -- set interface " + Name + " type=" + Type + " options:remote_ip=" + RemoteIp + " options:key=" + Vni))
            {
                _logger.LogInformation("Executing command: " + cmd.CommandText);
                cmd.Execute();
                _logger.LogInformation("Command>" + cmd.CommandText);
                _logger.LogInformation("Return Value = {0}", cmd.ExitStatus);
            }
            _logger.LogInformation("Disconnecting from device");
            sshclient.Disconnect();
        }
    }
}
