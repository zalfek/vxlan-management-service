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
    public class VXLANInterface : IVXLANInterface                                                                                                   
    {
        public string Name { get; set; }
        public string BridgeName { get; set; }
        public string Type { get; set; }
        public string RemoteIp { get; set; }
        public string Vni { get; set; }
        public string ManagementIp { get; set; }
        private readonly ILogger<IVXLANInterface> _logger;
        public string Username { get; set; }
        public string Key { get; set; }

        public VXLANInterface(string username, string key, string name, string type, string remoteIp, string vni, string bridgeName, string managementIp)
        {
            ManagementIp = managementIp;
            _logger = LoggerFactory.Create(logging => logging.AddConsole()).CreateLogger<IVXLANInterface>();
            Name = name;
            Type = type;
            RemoteIp = remoteIp;
            Vni = vni;
            BridgeName = bridgeName;
            Username = username;
            Key = key;
        }



        public void CleanUpVXLANInterface()
        {
            ConnectionInfo sSHConnectionInfo = new(ManagementIp, Username, new AuthenticationMethod[]{
             new PrivateKeyAuthenticationMethod(Username, new PrivateKeyFile[]{
                    new PrivateKeyFile(KeyKeeper.GetInstance().GetKeyLocation(Key))
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

        public void DeployVXLANInterface()
        {
            ConnectionInfo sSHConnectionInfo = new(ManagementIp, Username, new AuthenticationMethod[]{
             new PrivateKeyAuthenticationMethod(Username, new PrivateKeyFile[]{
                    new PrivateKeyFile(KeyKeeper.GetInstance().GetKeyLocation(Key))
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
