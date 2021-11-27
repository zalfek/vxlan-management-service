
using OverlayManagementService.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public interface IFirewall
    {
        public void AddException(IUser user);
        public void RemoveException(IUser user);

    }
}
