﻿using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OverlayManagementService.Repositories
{
    public interface IRepository
    {
        IOverlayNetwork SaveOverlayNetwork(string membership, IOverlayNetwork overlayNetwork);
        void DeleteOverlayNetwork(string membership);
        IOverlayNetwork UpdateOverlayNetwork(string membership, IOverlayNetwork overlayNetwork);
        IOverlayNetwork GetOverlayNetwork(string membership);
        IDictionary<string, IOverlayNetwork> GetAllNetworks();
        IEnumerable<IOverlayNetwork> GetAllSwitches();
        IOverlayNetwork GetOverlayNetworkByVni(string id);
        IOverlayNetwork GetOverlayNetwork(Claim claim);
    }
}
