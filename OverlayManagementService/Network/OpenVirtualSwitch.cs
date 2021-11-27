using Microsoft.Extensions.Logging;
using OverlayManagementService.Network;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public class OpenVirtualSwitch : IOpenVirtualSwitch
    {

        private readonly ILogger<OpenVirtualSwitch> _logger;
        private ConnectionInfo SSHConnectionInfo;
        public List<IBridge> Bridges { get; set; }
        public string PrivateIP;
        public string PublicIP;


        public OpenVirtualSwitch()
        {
            SSHConnectionInfo = new ConnectionInfo("192.168.56.103", "vagrant", new AuthenticationMethod[]{
                            new PasswordAuthenticationMethod("vagrant", "vagrant")
            });
            Bridges = new List<IBridge>();
        }


        public void AddBridge(IBridge bridge) {
            Bridges.Add(bridge);
        }


        public void DeployVXLANInterface(IVirtualMachine virtualMachine)
        {
            Bridges[0].DeployVXLANInterface(virtualMachine);
        }

        public void DeployOVSConnection(IVirtualMachine virtualMachine)
        {
            using (var sshclient = new SshClient(SSHConnectionInfo))
            {
                sshclient.Connect();
                Bridges.ForEach(br =>
               {
                   using (var cmd = sshclient.CreateCommand("sudo ovs-vsctl add-br " + br.Name))
                   {
                       cmd.Execute();
                       Console.WriteLine("Command>" + cmd.CommandText);
                       Console.WriteLine("Return Value = {0}", cmd.ExitStatus);
                   }
               });
                sshclient.Disconnect();
            }
        }

        public void CleanUpOVSConnection(IVirtualMachine virtualMachine)
        {
            Bridges.ForEach(br => {
                br.CleanUpBridge();
                Bridges.Remove(br);
                });
        }
    }
}
