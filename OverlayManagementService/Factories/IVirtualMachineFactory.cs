﻿using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Factories
{
    public interface IVirtualMachineFactory
    {
        public IVirtualMachine CreateVirtualMachine(Guid guid, string username, string switchKey, string managementIp, string vni, string destIP, string communicationIP);
    }
}
