using Microsoft.Extensions.Logging;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public class VXLANInterface : IVXLANInterface                                                                                                   
    {

        private readonly ILogger<IVXLANInterface> _logger;
        private ConnectionInfo SSHConnectionInfo;
        public VXLANInterface(string name, string type, string remoteIp, string key, IBridge parent)
        {
            SSHConnectionInfo = new ConnectionInfo("192.168.56.106", "vagrant", new AuthenticationMethod[]{
                            new PasswordAuthenticationMethod("vagrant", "vagrant")
            });

            _logger = new LoggerFactory().CreateLogger<IVXLANInterface>();
            Name = name;
            Type = type;
            RemoteIp = remoteIp;
            Key = key;
            Parent = parent;
        }

        private string Name { get; set; }
        private IBridge Parent { get; set; }
        private string Type { get; set; }
        private string RemoteIp { get; set; }
        private string Key { get; set; }
        private string OpenFlowPort { get; set; }
        private string SourcePort { get; set; }

        public void CleanUpVXLANInterface()
        {
            using (var sshclient = new SshClient(SSHConnectionInfo))
            {
                sshclient.Connect();
                using (var cmd = sshclient.CreateCommand("sudo ovs-vsctl del-port " + Parent.Name + " " + Name))
                {
                    cmd.Execute();
                    Console.WriteLine("Command>" + cmd.CommandText);
                    Console.WriteLine("Return Value = {0}", cmd.ExitStatus);
                }
                sshclient.Disconnect();
            }
        }

        public void DeployVXLANInterface()
        {
            using (var sshclient = new SshClient(SSHConnectionInfo))
            {
                sshclient.Connect();
                using (var cmd = sshclient.CreateCommand("sudo ovs-vsctl add-port " + Parent.Name + " " + Name + " -- set interface " + Name + " type " + Type + " options:remote_ip=" + RemoteIp + " options:key=" + Key))
                {
                    cmd.Execute();
                    Console.WriteLine("Command>" + cmd.CommandText);
                    Console.WriteLine("Return Value = {0}", cmd.ExitStatus);
                }
                sshclient.Disconnect();
            }
        }
    }
}
