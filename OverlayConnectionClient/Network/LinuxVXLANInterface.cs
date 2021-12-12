﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OverlayConnectionClient.Network
{
    public class LinuxVXLANInterface : ILinuxVXLANInterface
    {
        public string Name { get; set; }
        public string VNI { get; set; }
        public string DstPort { get; set; }
        public string DstIP { get; set; }
        public string LocalIP { get; set; }

        public LinuxVXLANInterface(string name, string vNI, string dstPort, string dstIP, string localIP)
        {
            
            Name = name;
            VNI = vNI;
            DstPort = dstPort;
            DstIP = dstIP;
            LocalIP = localIP;

        }

        public async void CleanUpInterface()
        {
           
            Process process = new Process {

             StartInfo = new ProcessStartInfo
             {
                FileName = "bash",
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute =false
             }

            };
            process.Start();
            await process.StandardInput.WriteLineAsync("sudo ip link del " + Name);
            Console.WriteLine(await process.StandardOutput.ReadLineAsync());
        }

        public async void DeployInterface()
        {

            Process process = new Process {

             StartInfo = new ProcessStartInfo
             {
                FileName = "bash",
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute =false
             }

            };
            process.Start();
            await process.StandardInput.WriteLineAsync("sudo ip link add " + Name + " type vxlan id " + VNI + " dstport " + DstPort + " srcport " + DstPort + " 4790");
            Console.WriteLine(await process.StandardOutput.ReadLineAsync());
            await process.StandardInput.WriteLineAsync("sudo ip addr add " + LocalIP + " dev " + Name);
            Console.WriteLine(await process.StandardOutput.ReadLineAsync());
            await process.StandardInput.WriteLineAsync("sudo ip link set " + Name + " up");
            Console.WriteLine(await process.StandardOutput.ReadLineAsync());
            await process.StandardInput.WriteLineAsync("bridge fdb add 00:00:00:00:00:00 dev " + Name + " dst" + DstIP);
            Console.WriteLine(await process.StandardOutput.ReadLineAsync());
        }
    }
}