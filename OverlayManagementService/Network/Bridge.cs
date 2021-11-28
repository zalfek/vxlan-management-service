
using Microsoft.Extensions.Logging;
using Renci.SshNet;
using System;
using System.Collections.Generic;

namespace OverlayManagementService.Network
{
    public class Bridge: IBridge
    {
        private readonly ILogger<IBridge> _logger;

        public Bridge(string name, string vni, ConnectionInfo sSHConnectionInfo)
        {
            _logger = new LoggerFactory().CreateLogger<IBridge>();
            Name = name;
            VNI = vni;
            SSHConnectionInfo = sSHConnectionInfo;
        }

        public string VNI { get; set; }
        public string Name { get; set; }
        public ConnectionInfo SSHConnectionInfo;
        public List<IVXLANInterface> VXLANInterfaces { get; set; }

        public void DeployVXLANInterface(IVirtualMachine virtualMachine)
        {
            IVXLANInterface vXLANInterface = new VXLANInterface(Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 14),
                "vxlan",
                virtualMachine.IPAddress,
                VNI,
                this
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
            using (var sshclient = new SshClient(SSHConnectionInfo))
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
