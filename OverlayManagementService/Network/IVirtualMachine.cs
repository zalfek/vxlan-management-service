﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public interface IVirtualMachine
    {
        public void DeployVMConnection(IOpenVirtualSwitch openVirtualSwitch);
        public void CleanUpVMConnection(IOpenVirtualSwitch openVirtualSwitch);
    }
}
