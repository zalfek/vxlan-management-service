using OverlayManagementService.Dtos;
using OverlayManagementService.Network;
using System;
using System.Net;

namespace OverlayManagementService.Factories
{
    public class OpenVirtualSwitchFactory : IOpenVirtualSwitchFactory
    {
        public IOpenVirtualSwitch CreateSwitch(OvsRegistration ovsRegistration)
        {


            if (System.Net.IPAddress.TryParse(ovsRegistration.ManagementIp, out System.Net.IPAddress managementIp))
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
                managementIp = Dns.GetHostEntry(ovsRegistration.ManagementIp).AddressList[0];
            }

            if (System.Net.IPAddress.TryParse(ovsRegistration.ManagementIp, out System.Net.IPAddress privateIP))
            {
                switch (privateIP.AddressFamily)
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
                privateIP = Dns.GetHostEntry(ovsRegistration.PrivateIP).AddressList[0];
            }

            if (System.Net.IPAddress.TryParse(ovsRegistration.ManagementIp, out System.Net.IPAddress publicIP))
            {
                switch (publicIP.AddressFamily)
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
                publicIP = Dns.GetHostEntry(ovsRegistration.PublicIP).AddressList[0];
            }

            return new OpenVirtualSwitch(
                ovsRegistration.Key,
                managementIp.ToString(),
                privateIP.ToString(),
                publicIP.ToString()
                );
        }
    }
}
