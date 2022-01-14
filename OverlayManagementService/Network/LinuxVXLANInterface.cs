using Microsoft.Extensions.Logging;
using OverlayManagementService.Services;
using Renci.SshNet;

namespace OverlayManagementService.Network
{
    /// <summary>
    /// Class which encapsulates VXLAN interface deployment and cleanup functionality on target device.
    /// </summary>
    public class LinuxVXLANInterface : ILinuxVXLANInterface
    {
        private readonly ILogger<ILinuxVXLANInterface> _logger;
        public readonly string Name;
        public readonly string VNI;
        public readonly string DstPort;
        public readonly string DstIP;

        public LinuxVXLANInterface(string name, string vNI, string dstPort, string dstIP)
        {
            _logger = LoggerFactory.Create(logging => logging.AddConsole()).CreateLogger<ILinuxVXLANInterface>();
            Name = name;
            VNI = vNI;
            DstPort = dstPort;
            DstIP = dstIP;
        }

        /// <summary>
        /// Method deletes the VXLAN interface(towards Open Virtual Switch) from target device.
        /// </summary>
        /// <param name="username">username to access the target device</param>
        /// <param name="key">GUID of the machine which was used for naming the private key for this targetdevice</param>
        /// <param name="managementIp">Management ip ov the target device</param>
        public void CleanUpInterface(string username, string key, string managementIp)
        {

            ConnectionInfo sSHConnectionInfo = new(managementIp, username, new AuthenticationMethod[]{
             new PrivateKeyAuthenticationMethod(username, new PrivateKeyFile[]{
                    new PrivateKeyFile(KeyKeeper.GetInstance().GetKeyLocation(key))
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

        /// <summary>
        /// Method deploys the VXLAN interface(towards Open Virtual Switch) to a target device.
        /// </summary>
        /// <param name="username">username to access the target device</param>
        /// <param name="key">GUID of the machine which was used for naming the private key for this targetdevice</param>
        /// <param name="managementIp">Management ip ov the target device</param>
        public void DeployInterface(string username, string key, string managementIp, string vxlanIp)
        {

            ConnectionInfo sSHConnectionInfo = new(managementIp, username, new AuthenticationMethod[]{
             new PrivateKeyAuthenticationMethod(username, new PrivateKeyFile[]{
                    new PrivateKeyFile(KeyKeeper.GetInstance().GetKeyLocation(key))
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
