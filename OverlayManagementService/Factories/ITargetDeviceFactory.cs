using OverlayManagementService.Network;
using System;

namespace OverlayManagementService.Factories
{
    public interface ITargetDeviceFactory
    {
        public ITargetDevice CreateTargetDevice(Guid guid, string username, string switchKey, string managementIp, string vni, string destIP, string communicationIP);
    }
}
