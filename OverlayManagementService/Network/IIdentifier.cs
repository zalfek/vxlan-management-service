using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public interface IIdentifier
    {
        public string ReserveVNI();
        public void ReleaseVNI(string vni);
    }
}
