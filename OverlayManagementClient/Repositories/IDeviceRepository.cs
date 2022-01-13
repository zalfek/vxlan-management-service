using OverlayManagementClient.Models;
using System;

namespace OverlayManagementClient.Repositories
{
    public interface IDeviceRepository
    {
        public void AddDeviceAsync(VmConnection vmConnection);
        public void RemoveDeviceAsync(string groupid, Guid guid);
    }
}
