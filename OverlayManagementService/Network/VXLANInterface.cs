using Microsoft.Extensions.Logging;
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

        public VXLANInterface(string name, string type, string remoteIp, string vni, string bridgeName, string managementIp)
        {
            ManagementIp = managementIp;
            _logger = new LoggerFactory().CreateLogger<IVXLANInterface>();
            Name = name;
            Type = type;
            RemoteIp = remoteIp;
            Vni = vni;
            BridgeName = bridgeName;
        }



        public void CleanUpVXLANInterface()
        {
            ConnectionInfo sSHConnectionInfo = new ConnectionInfo(ManagementIp, "vagrant", new AuthenticationMethod[]{
                            new PasswordAuthenticationMethod("vagrant", "vagrant")});

            using (var sshclient = new SshClient(sSHConnectionInfo))
            {
                sshclient.Connect();
                using (var cmd = sshclient.CreateCommand("sudo ovs-vsctl del-port " + BridgeName + " " + Name))
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
            ConnectionInfo sSHConnectionInfo = new ConnectionInfo(ManagementIp, "vagrant", new AuthenticationMethod[]{
                            new PasswordAuthenticationMethod("vagrant", "vagrant")});

            using (var sshclient = new SshClient(sSHConnectionInfo))
            {
                sshclient.Connect();
                using (var cmd = sshclient.CreateCommand("sudo ovs-vsctl add-port " + BridgeName + " " + Name + " -- set interface " + Name + " type=" + Type + " options:remote_ip=" + RemoteIp + " options:key=" + Vni))
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
