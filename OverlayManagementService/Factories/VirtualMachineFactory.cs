using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OverlayManagementService.Factories
{
    public class VirtualMachineFactory : IVirtualMachineFactory
    {
        public IVirtualMachine CreateVirtualMachine(Guid guid, string username, string key, string managementAddress, string vni, string destAddress, string communicationAddress)
        {

            if (System.Net.IPAddress.TryParse(managementAddress, out System.Net.IPAddress managementIp))
            {
                switch (managementIp.AddressFamily)
                {
                    case System.Net.Sockets.AddressFamily.InterNetwork:
                        // we have IPv4
                        break;
                    default:
                        throw new Exception("Unsupported address");
                }
            }
            else
            {
                managementIp = Dns.GetHostEntry(managementAddress).AddressList[0];
            }

            if (System.Net.IPAddress.TryParse(destAddress, out System.Net.IPAddress destIP))
            {
                switch (destIP.AddressFamily)
                {
                    case System.Net.Sockets.AddressFamily.InterNetwork:
                        // we have IPv4
                        break;
                    default:
                        throw new Exception("Unsupported address");
                }
            }
            else
            {
                destIP = Dns.GetHostEntry(destAddress).AddressList[0];
            }

            if (System.Net.IPAddress.TryParse(communicationAddress, out System.Net.IPAddress communicationIp))
            {
                switch (communicationIp.AddressFamily)
                {
                    case System.Net.Sockets.AddressFamily.InterNetwork:
                        // we have IPv4
                        break;
                    default:
                        throw new Exception("Unsupported address");
                }
            }
            else
            {
                communicationIp = Dns.GetHostEntry(communicationAddress).AddressList[0];
            }

            return new VirtualMachine(
                guid,
                username,
                key,
                managementIp.ToString(),
                vni,
                destIP.ToString(),
                communicationIp.ToString()
                );
        }
    }
}
