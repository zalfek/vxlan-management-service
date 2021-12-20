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
        public Task DeleteNetworkAsync(string groupId);
        public Task<OverlayNetwork> EditNetworkAsync(OverlayNetwork OverlayNetwork);
        public Task<IEnumerable<OverlayNetwork>> GetNetworksAsync();
        public Task<OverlayNetwork> GetNetworkAsync(string id);
        public Task<IEnumerable<OpenVirtualSwitch>> GetSwitchesAsync();
        public Task<OpenVirtualSwitch> GetSwitchAsync(string key);
        public void AddSwitchAsync(OpenVirtualSwitch openVirtualSwitch);
        public void AddMachineAsync(VmConnection vmConnection);
        public void RemoveMachineAsync(string groupid, Guid guid);
        public Task DeleteSwitchAsync(string key);
    }
}
