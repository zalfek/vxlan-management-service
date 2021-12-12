using OverlayConnectionClient.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OverlayConnectionClient.Repositories
{
    public interface IRepository
    {
        public ILinuxVXLANInterface SaveInterface(string groupId, ILinuxVXLANInterface linuxVXLANInterface);
        void DeleteInterface(string groupId);
        public ILinuxVXLANInterface GetVXLANInterface(string groupId);
    }
}
