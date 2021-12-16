
using Microsoft.Extensions.Logging;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OverlayManagementService.Network
{
    public class Bridge: IBridge
    {
        private readonly ILogger<IBridge> _logger;

        public Bridge(string name, string vni, string managementIp)
        {
            _logger = new LoggerFactory().CreateLogger<IBridge>();
            Name = name;
            Vni = vni;
            ManagementIp = managementIp;
            VXLANInterfaces = new List<IVXLANInterface>();
        }

        public string Vni { get; set; }
        public string Name { get; set; }
        public List<IVXLANInterface> VXLANInterfaces { get; set; }
        public string ManagementIp { get; set; }

        public void DeployVXLANInterface(IVirtualMachine virtualMachine)
        {
            IVXLANInterface vXLANInterface = new VXLANInterface(
                Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 14),
                "vxlan",
                virtualMachine.CommunicationIP,
                Vni,
                Name,
                ManagementIp
                );
            vXLANInterface.DeployVXLANInterface();
            VXLANInterfaces.Add(vXLANInterface);
        }


        public void DeployClientVXLANInterface(string ip)
        {
            IVXLANInterface vXLANInterface = new VXLANInterface(
                Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 14),
                "vxlan",
                ip,
                Vni,
                Name,
                ManagementIp
                );
            vXLANInterface.DeployVXLANInterface();
            VXLANInterfaces.Add(vXLANInterface);
        }

        public void CleanUpBridge()
        {
            throw new System.NotImplementedException();
        }

        public void DeployBridge()
        {
            ConnectionInfo sSHConnectionInfo = new ConnectionInfo(ManagementIp, "vagrant", new AuthenticationMethod[]{
                            new PasswordAuthenticationMethod("vagrant", "vagrant")});
            using (var sshclient = new SshClient(sSHConnectionInfo))
            {
                sshclient.Connect();
                    using (var cmd = sshclient.CreateCommand("sudo ovs-vsctl add-br " + Name))
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
