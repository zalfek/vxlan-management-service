using Microsoft.Extensions.Logging;
using Renci.SshNet;

namespace OverlayManagementService.Network
{
    /// <summary>
    /// Class encapsulates IP tables functionality for allowing client connections.
    /// </summary>
    public class Firewall : IFirewall
    {
        private readonly ILogger<IFirewall> _logger;
        public string _managementIp;
        public Firewall(string managementIp)
        {
            _logger = _logger = LoggerFactory.Create(logging => logging.AddConsole()).CreateLogger<IFirewall>();
            _managementIp = managementIp;
        }

        /// <summary>
        /// Method adds an exception for provided ip address to allow in7out connections for client.
        /// </summary>
        /// <param name="ip"></param>
        public void AddException(string ip)
        {
            ConnectionInfo sSHConnectionInfo = new(_managementIp, "vagrant", new AuthenticationMethod[]{
                            new PasswordAuthenticationMethod("vagrant", "vagrant")});
            using var sshclient = new SshClient(sSHConnectionInfo);
            sshclient.Connect();
            using (var cmd = sshclient.CreateCommand("sudo iptables -A INPUT -s " + ip + " -j ACCEPT"))
            {
                cmd.Execute();
                _logger.LogInformation("Command>" + cmd.CommandText);
                _logger.LogInformation("Return Value = {0}", cmd.ExitStatus);
            }
            using (var cmd = sshclient.CreateCommand("sudo iptables -A OUTPUT -d " + ip + " -j ACCEPT"))
            {
                cmd.Execute();
                _logger.LogInformation("Command>" + cmd.CommandText);
                _logger.LogInformation("Return Value = {0}", cmd.ExitStatus);
            }

            using (var cmd = sshclient.CreateCommand("sudo iptables -A FORWARD -d " + ip + " -j ACCEPT"))
            {
                cmd.Execute();
                _logger.LogInformation("Command>" + cmd.CommandText);
                _logger.LogInformation("Return Value = {0}", cmd.ExitStatus);
            }

            sshclient.Disconnect();
        }

        /// <summary>
        /// Method removes existing exception for the provided ip address.
        /// </summary>
        /// <param name="ip"></param>
        public void RemoveException(string ip)
        {
            ConnectionInfo sSHConnectionInfo = new(_managementIp, "vagrant", new AuthenticationMethod[]{
                            new PasswordAuthenticationMethod("vagrant", "vagrant")});
            using var sshclient = new SshClient(sSHConnectionInfo);
            sshclient.Connect();
            using (var cmd = sshclient.CreateCommand("sudo iptables -D INPUT -s " + ip + " -j ACCEPT"))
            {
                cmd.Execute();
                _logger.LogInformation("Command>" + cmd.CommandText);
                _logger.LogInformation("Return Value = {0}", cmd.ExitStatus);
            }
            using (var cmd = sshclient.CreateCommand("sudo iptables -D OUTPUT -d " + ip + " -j ACCEPT"))
            {
                cmd.Execute();
                _logger.LogInformation("Command>" + cmd.CommandText);
                _logger.LogInformation("Return Value = {0}", cmd.ExitStatus);
            }

            using (var cmd = sshclient.CreateCommand("sudo iptables -D FORWARD -d " + ip + " -j ACCEPT"))
            {
                cmd.Execute();
                _logger.LogInformation("Command>" + cmd.CommandText);
                _logger.LogInformation("Return Value = {0}", cmd.ExitStatus);
            }

            sshclient.Disconnect();
        }
    }
}
