using OverlayManagementService.Dtos;
using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OverlayManagementService.Factories
{
    public class OpenVirtualSwitchFactory : IOpenVirtualSwitchFactory
    {
        public IOpenVirtualSwitch CreateSwitch(OvsRegistration ovsRegistration)
        {
            return new OpenVirtualSwitch(
                ovsRegistration.Key,
                Dns.GetHostEntry(ovsRegistration.ManagementIp).AddressList.GetValue(0).ToString(),
                Dns.GetHostEntry(ovsRegistration.PrivateIP).AddressList.GetValue(0).ToString(),
                Dns.GetHostEntry(ovsRegistration.PublicIP).AddressList.GetValue(0).ToString()
                );
        }
    }
}
