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
    public class LinuxVXLANInterface : ILinuxVXLANInterface
    {
        private readonly ILogger<ILinuxVXLANInterface> _logger;
        public readonly string Name;
        public readonly string VNI;
        public readonly string DstPort;
        public readonly string DstIP;
        public readonly string ManagementIp;
        public string Username;
        public string Key;

        public LinuxVXLANInterface(string username, string key, string name, string vNI, string dstPort, string dstIP, string managementIp)
        {
            _logger = LoggerFactory.Create(logging => logging.AddConsole()).CreateLogger<ILinuxVXLANInterface>();
            Name = name;
            VNI = vNI;
            DstPort = dstPort;
            DstIP = dstIP;
            ManagementIp = managementIp;
            Username = username;
            Key = key;
        }

        public void CleanUpInterface()
        {

            ConnectionInfo sSHConnectionInfo = new(ManagementIp, Username, new AuthenticationMethod[]{
             new PrivateKeyAuthenticationMethod(Username, new PrivateKeyFile[]{
                    new PrivateKeyFile(KeyKeeper.GetInstance().GetKeyLocation(Key))
                }),
            });

            using var sshclient = new SshClient(sSHConnectionInfo);
            sshclient.Connect();
            using (var cmd = sshclient.CreateCommand("sudo ip link del " + Name))
            {
                cmd.Execute();
                _logger.LogInformation("Command>" + cmd.CommandText);
                _logger.LogInformation("Return Value = {0}", cmd.ExitStatus);
            }
            sshclient.Disconnect();
        }

        public void DeployInterface(string vxlanIp)
        {

            ConnectionInfo sSHConnectionInfo = new(ManagementIp, Username, new AuthenticationMethod[]{
             new PrivateKeyAuthenticationMethod(Username, new PrivateKeyFile[]{
                    new PrivateKeyFile(KeyKeeper.GetInstance().GetKeyLocation(Key))
                }),
            });

            using var sshclient = new SshClient(sSHConnectionInfo);
            sshclient.Connect();
            using (var cmd = sshclient.CreateCommand("sudo ip link add " + Name + " type vxlan id " + VNI + " dstport " + DstPort + " srcport " + DstPort + " 4790"))
            {
                cmd.Execute();
                _logger.LogInformation("Command>" + cmd.CommandText);
                _logger.LogInformation("Return Value = {0}", cmd.ExitStatus);
            }
            using (var cmd = sshclient.CreateCommand("sudo ip addr add " + vxlanIp + "/24 dev " + Name))
            {
                cmd.Execute();
                _logger.LogInformation("Command>" + cmd.CommandText);
                _logger.LogInformation("Return Value = {0}", cmd.ExitStatus);
            }
            using (var cmd = sshclient.CreateCommand("sudo ip link set " + Name + " up"))
            {
                cmd.Execute();
                _logger.LogInformation("Command>" + cmd.CommandText);
                _logger.LogInformation("Return Value = {0}", cmd.ExitStatus);
            }
            using (var cmd = sshclient.CreateCommand("sudo bridge add 00:00:00:00:00:00 dev " + Name + " dst " + DstIP))
            {
                cmd.Execute();
                _logger.LogInformation("Command>" + cmd.CommandText);
                _logger.LogInformation("Return Value = {0}", cmd.ExitStatus);
            }
            sshclient.Disconnect();
        }
    }
}
