using OverlayManagementClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementClient.Repositories
{
    public interface ISwitchRepository
    {
        public Task<IEnumerable<OpenVirtualSwitch>> GetSwitchesAsync();
        public Task<OpenVirtualSwitch> GetSwitchAsync(string key);
        public void AddSwitchAsync(OvsRegistration ovsRegistration);
        public Task DeleteSwitchAsync(string key);

    }
}
