using Microsoft.Extensions.Logging;
using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public class OpenVirtualSwitch : IOpenVirtualSwitch
    {

        private readonly ILogger<OpenVirtualSwitch> _logger;
        private IVXLANInterface vXLANInterface;
        public void CreateConnection(IVirtualMachine virtualMachine)
        {
            throw new NotImplementedException();
        }

        public void RemoveConnection(IVirtualMachine virtualMachine)
        {
            throw new NotImplementedException();
        }
    }
}
