using OverlayConnectionClient.Network;
using System.Collections.Generic;

namespace OverlayConnectionClient.Repositories
{
    public interface IInterfaceRepository
    {
        public ILinuxVXLANInterface SaveInterface(string groupId, ILinuxVXLANInterface linuxVXLANInterface);
        void DeleteInterface(string groupId);
        public ILinuxVXLANInterface GetVXLANInterface(string groupId);
        public IDictionary<string, ILinuxVXLANInterface> GetAllInterfaces();
    }
}
