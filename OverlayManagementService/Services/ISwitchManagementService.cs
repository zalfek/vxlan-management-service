using OverlayManagementService.Dtos;
using OverlayManagementService.Network;
using System.Collections.Generic;

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
