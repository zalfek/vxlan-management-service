using OverlayManagementService.Dtos;
using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Services
{
    public interface ISwitchManagementService
    {
        public IOpenVirtualSwitch AddSwitch(OvsRegistration ovsRegistration);
        public IEnumerable<IOpenVirtualSwitch> GetAllSwitches();
        public IOpenVirtualSwitch GetSwitch(string key);
        public void RemoveSwitch(string key);
    }
}
