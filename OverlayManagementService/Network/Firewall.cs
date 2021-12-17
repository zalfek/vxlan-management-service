using Microsoft.Extensions.Logging;
using OverlayManagementService.Dtos;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public class Firewall : IFirewall
    {
        private readonly ILogger<Firewall> _logger;
        private readonly string _managementIp;
        public Firewall(string managementIp)
        {
            _logger = _logger = new LoggerFactory().CreateLogger<Firewall>();
            _managementIp = managementIp;
        }

        public void AddException(string ip)
        {
            ConnectionInfo sSHConnectionInfo = new(_managementIp, "vagrant", new AuthenticationMethod[]{
                            new PasswordAuthenticationMethod("vagrant", "vagrant")});
            using var sshclient = new SshClient(sSHConnectionInfo);
            sshclient.Connect();
            using (var cmd = sshclient.CreateCommand("iptables -A INPUT -s " + ip + " -j ACCEPT"))
            {
                cmd.Execute();
                _logger.LogInformation("Command>" + cmd.CommandText);
                _logger.LogInformation("Return Value = {0}", cmd.ExitStatus);
            }
            using (var cmd = sshclient.CreateCommand("iptables -A OUTPUT -d " + ip + " -j ACCEPT"))
            {
                cmd.Execute();
                _logger.LogInformation("Command>" + cmd.CommandText);
                _logger.LogInformation("Return Value = {0}", cmd.ExitStatus);
            }

            sshclient.Disconnect();
        }

        public void RemoveException(string ip)
        {
            ConnectionInfo sSHConnectionInfo = new(_managementIp, "vagrant", new AuthenticationMethod[]{
                            new PasswordAuthenticationMethod("vagrant", "vagrant")});
            using var sshclient = new SshClient(sSHConnectionInfo);
            sshclient.Connect();
            using (var cmd = sshclient.CreateCommand("iptables -D INPUT -s " + ip + " -j ACCEPT"))
            {
                cmd.Execute();
                _logger.LogInformation("Command>" + cmd.CommandText);
                _logger.LogInformation("Return Value = {0}", cmd.ExitStatus);
            }
            using (var cmd = sshclient.CreateCommand("iptables -D OUTPUT -d " + ip + " -j ACCEPT"))
            {
                cmd.Execute();
                _logger.LogInformation("Command>" + cmd.CommandText);
                _logger.LogInformation("Return Value = {0}", cmd.ExitStatus);
            }

            sshclient.Disconnect();
        }
    }
}
