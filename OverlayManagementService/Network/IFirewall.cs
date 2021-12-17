
using OverlayManagementService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public interface IFirewall
    {
        public void AddException(string ip);
        public void RemoveException(string ip);
    }
}
