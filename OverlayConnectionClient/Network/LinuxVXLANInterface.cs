using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OverlayConnectionClient.Network
{
    public class LinuxVXLANInterface : ILinuxVXLANInterface
    {
        private readonly ILogger<LinuxVXLANInterface> _logger;
        public string Name { get; set; }
        public string VNI { get; set; }
        public string DstPort { get; set; }
        public string DstIP { get; set; }
        public string LocalIP { get; set; }

        public LinuxVXLANInterface(string name, string vNI, string dstPort, string dstIP, string localIP)
        {
            _logger = new LoggerFactory().CreateLogger<LinuxVXLANInterface>();
            Name = name;
            VNI = vNI;
            DstPort = dstPort;
            DstIP = dstIP;
            LocalIP = localIP;

        }

        public async void CleanUpInterface()
        {

            Process process = new()
            {

                StartInfo = new ProcessStartInfo
                {
                    FileName = "bash",
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }

            };
            process.Start();
            using (StreamWriter sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("sudo ip link del " + Name);
                }
            }
            _logger.LogInformation("Standart output: " + await process.StandardOutput.ReadLineAsync());
        }

        public async void DeployInterface()
        {

            Process process = new()
            {

                StartInfo = new ProcessStartInfo
                {
                    FileName = "bash",
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }

            };
            process.Start();

            using (StreamWriter sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("sudo ip link add " + Name + " type vxlan id " + VNI + " dstport " + DstPort + " srcport " + DstPort + " 4790");
                    sw.WriteLine("sudo ip addr add " + LocalIP + "/24 dev " + Name);
                    sw.WriteLine("sudo ip link set " + Name + " up");
                    sw.WriteLine("sudo bridge fdb add 00:00:00:00:00:00 dev " + Name + " dst " + DstIP);
                }
            }
            _logger.LogInformation("Standart output: " + await process.StandardOutput.ReadLineAsync());

        }
    }
}
