using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public class LinuxVXLANInterface : ILinuxVXLANInterface
    {
        public readonly string Name;
        public readonly string VNI;
        public readonly string DstPort;
        public readonly string DstIP;
        public readonly string LocalIP;
        private ConnectionInfo SSHConnectionInfo;
        public LinuxVXLANInterface(string name, string vNI, string dstPort, string dstIP, string localIP)
        {
            SSHConnectionInfo = new ConnectionInfo("192.168.56.106", "vagrant", new AuthenticationMethod[]{
                            new PasswordAuthenticationMethod("vagrant", "vagrant")
            });

            Name = name;
            VNI = vNI;
            DstPort = dstPort;
            DstIP = dstIP;
            LocalIP = localIP;
        }

        public void CleanUpInterface()
        {
            using (var sshclient = new SshClient(SSHConnectionInfo))
            {
                sshclient.Connect();
                using (var cmd = sshclient.CreateCommand("sudo ip link del " + Name))
                {
                    cmd.Execute();
                    Console.WriteLine("Command>" + cmd.CommandText);
                    Console.WriteLine("Return Value = {0}", cmd.ExitStatus);
                }
                sshclient.Disconnect();
            }
        }

        public void DeployInterface()
        {
            using (var sshclient = new SshClient(SSHConnectionInfo))
            {
                sshclient.Connect();
                using (var cmd = sshclient.CreateCommand("sudo ip link add " + Name + " type vxlan id " + VNI + " dstport " + DstPort + " srcport " + DstPort + " 4790"))
                {
                    cmd.Execute();
                    Console.WriteLine("Command>" + cmd.CommandText);
                    Console.WriteLine("Return Value = {0}", cmd.ExitStatus);
                }
                using (var cmd = sshclient.CreateCommand("sudo ip addr add " + LocalIP + " dev " + Name))
                {
                    cmd.Execute();
                    Console.WriteLine("Command>" + cmd.CommandText);
                    Console.WriteLine("Return Value = {0}", cmd.ExitStatus);
                }
                using (var cmd = sshclient.CreateCommand("sudo ip link set " + Name + " up" ))
                {
                    cmd.Execute();
                    Console.WriteLine("Command>" + cmd.CommandText);
                    Console.WriteLine("Return Value = {0}", cmd.ExitStatus);
                }
                using (var cmd = sshclient.CreateCommand("bridge fdb add 00:00:00:00:00:00 dev " + Name + " dst" + DstIP))
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
