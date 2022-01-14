using Microsoft.Extensions.Logging;
using OverlayManagementService.Factories;
using OverlayManagementService.Services;
using Renci.SshNet;
using System.Collections.Generic;

namespace OverlayManagementService.Network
{
    /// <summary>
    /// Class which encapsulates Open Virtual Switch functionality. Each VNI has a separate Bridge
    /// </summary>
    public class Bridge : IBridge
    {
        private readonly ILogger<IBridge> _logger;
        private readonly IVxlanInterfaceFactory _vxlanInterfaceFactory;
        public string Vni { get; set; }
        public string Name { get; set; }
        public List<IVXLANInterface> VXLANInterfaces { get; set; }
        public string ManagementIp { get; set; }
        public string Username { get; set; }
        public string Key { get; set; }

        public Bridge(string username, string key, string name, string vni, string managementIp)
        {
            _logger = LoggerFactory.Create(logging => logging.AddConsole()).CreateLogger<IBridge>();
            Name = name;
            Vni = vni;
            ManagementIp = managementIp;
            VXLANInterfaces = new List<IVXLANInterface>();
            Username = username;
            Key = key;
            _vxlanInterfaceFactory = new VxlanInterfaceFactory();
        }

        /// <summary>
        /// Triggers deployment of the VXLAN interface on the bridge.
        /// </summary>
        /// <param name="destIp">Destination IP address</param>
        public void DeployVXLANInterface(string destIp)
        {
            _logger.LogInformation("Creating new vxlan interface");
            IVXLANInterface vXLANInterface = _vxlanInterfaceFactory.CreateInterface(destIp, Vni, Name);
            _logger.LogInformation("Deploying vxlan interface");
            vXLANInterface.DeployVXLANInterface(Username, Key, ManagementIp);
            VXLANInterfaces.Add(vXLANInterface);
        }

        /// <summary>
        /// Triggers deployment of VXLAN interface towards client.
        /// </summary>
        /// <param name="destIp">Destination IP address</param>
        public void DeployClientVXLANInterface(string destIp)
        {
            _logger.LogInformation("Creating new vxlan interface");
            IVXLANInterface vXLANInterface = _vxlanInterfaceFactory.CreateInterface(destIp, Vni, Name);
            _logger.LogInformation("Deploying vxlan interface");
            vXLANInterface.DeployVXLANInterface(Username, Key, ManagementIp);
            VXLANInterfaces.Add(vXLANInterface);
        }

        /// <summary>
        // Deletes the bridge from the Open Virtual Switch
        /// </summary>
        public void CleanUpBridge()
        {
            ConnectionInfo sSHConnectionInfo = new(ManagementIp, Username, new AuthenticationMethod[]{
             new PrivateKeyAuthenticationMethod(Username, new PrivateKeyFile[]{
                    new PrivateKeyFile(KeyKeeper.GetInstance().GetKeyLocation(Key))
                }),
            });
            using var sshclient = new SshClient(sSHConnectionInfo);
            sshclient.Connect();
            using (var cmd = sshclient.CreateCommand("sudo ovs-vsctl del-br " + Name))
            {
                cmd.Execute();
                _logger.LogInformation("Command>" + cmd.CommandText);
                _logger.LogInformation("Return Value = {0}", cmd.ExitStatus);
            }

            sshclient.Disconnect();
        }

        /// <summary>
        /// Deploys the bridge to Open Virtual switch
        /// </summary>
        public void DeployBridge()
        {
            ConnectionInfo sSHConnectionInfo = new(ManagementIp, Username, new AuthenticationMethod[]{
             new PrivateKeyAuthenticationMethod(Username, new PrivateKeyFile[]{
                    new PrivateKeyFile(KeyKeeper.GetInstance().GetKeyLocation(Key))
                }),
            });
            using var sshclient = new SshClient(sSHConnectionInfo);
            sshclient.Connect();
            using (var cmd = sshclient.CreateCommand("sudo ovs-vsctl add-br " + Name))
            {
                cmd.Execute();
                _logger.LogInformation("Command>" + cmd.CommandText);
                _logger.LogInformation("Return Value = {0}", cmd.ExitStatus);
            }

            sshclient.Disconnect();
        }

        /// <summary>
        /// Triggers cleanup of the client VXLAN interface on Open Virtual Switch
        /// </summary>
        /// <param name="ip">Ip address of a client</param>
        public void CleanUpClientVXLANInterface(string ip)
        {
            _logger.LogInformation("Searching for vxlan interface");
            IVXLANInterface vXLANInterface = VXLANInterfaces.Find(x => x.RemoteIp == ip);
            _logger.LogInformation("Removing vxlan interface");
            vXLANInterface.CleanUpVXLANInterface(Username, Key, ManagementIp);
            VXLANInterfaces.Remove(vXLANInterface);
        }

        /// <summary>
        /// Triggers cleanup of a target device VXAN interface on Open Virtual switch.
        /// </summary>
        /// <param name="ip">Ip address of target device</param>
        public void CleanUpTargetVXLANInterface(string ip)
        {
            _logger.LogInformation("Searching for vxlan interface");
            IVXLANInterface vXLANInterface = VXLANInterfaces.Find(x => x.RemoteIp == ip);
            _logger.LogInformation("Removing vxlan interface");
            vXLANInterface.CleanUpVXLANInterface(Username, Key, ManagementIp);
            VXLANInterfaces.Remove(vXLANInterface);
        }

    }

}
