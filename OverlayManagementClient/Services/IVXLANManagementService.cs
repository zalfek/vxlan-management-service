using OverlayManagementClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementClient.Services
{
    public interface IVXLANManagementService
    {


        public Task<OverlayNetwork> AddNetworkAsync(OVSConnection oVSConnection);
        public Task DeleteNetworkAsync(int id);
        public Task<OverlayNetwork> EditNetworkAsync(OverlayNetwork OverlayNetwork);
        public Task<IEnumerable<OverlayNetwork>> GetNetworksAsync();
        public Task<OverlayNetwork> GetNetworkAsync(string id);
        public Task<IEnumerable<OpenVirtualSwitch>> GetSwitchesAsync();
        public Task<OpenVirtualSwitch> GetSwitchAsync(string key);
        void AddSwitchAsync(OpenVirtualSwitch openVirtualSwitch);
    }
}
