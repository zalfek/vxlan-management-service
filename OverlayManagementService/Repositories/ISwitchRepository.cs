using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Repositories
{
    public interface ISwitchRepository
    {
        public void DeleteSwitch(string key);
        public IOpenVirtualSwitch GetSwitch(string key);
        public IOpenVirtualSwitch SaveSwitch(IOpenVirtualSwitch openVirtualSwitch);
        public IOpenVirtualSwitch UpdateSwitch(IOpenVirtualSwitch openVirtualSwitch);
        public IDictionary<string, IOpenVirtualSwitch> GetAllSwitches();

    }
}
