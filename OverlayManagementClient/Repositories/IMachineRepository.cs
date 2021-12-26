using OverlayManagementClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementClient.Repositories
{
    public interface IMachineRepository
    {
        public void AddMachineAsync(VmConnection vmConnection);
        public void RemoveMachineAsync(string groupid, Guid guid);
    }
}
