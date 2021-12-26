﻿using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Factories
{
    public interface IVxlanInterfaceFactory
    {
        public IVXLANInterface CreateInterface(string username, string key, string name, string type, string remoteIp, string vni, string bridgeName, string managementIp);
    }
}