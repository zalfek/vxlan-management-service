using OverlayManagementClient.Models;
using System;
using System.Collections.Generic;

namespace OverlayManagementClient.Services
{
    public interface IVXLANManagementService
    {
        public OverlayNetwork AddNetwork(OVSConnection oVSConnection);
        public void DeleteNetwork(string groupId);
        public OverlayNetwork EditNetwork(OverlayNetwork OverlayNetwork);
        public IEnumerable<OverlayNetwork> GetNetworks();
        public OverlayNetwork GetNetwork(string id);
        public IEnumerable<OpenVirtualSwitch> GetSwitches();
        public OpenVirtualSwitch GetSwitch(string key);
        public void AddSwitch(OvsRegistration ovsRegistration);
        public void AddTargetDevice(VmConnection vmConnection);
        public void RemoveTargetDevice(string groupid, Guid guid);
        public void DeleteSwitch(string key);
    }
}
