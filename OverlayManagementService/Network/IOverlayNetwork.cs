﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public interface IOverlayNetwork
    {

        public void DeployNetwork();
        public void CleanUpNetwork();
    }
}
