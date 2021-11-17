using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public class Veth : IVeth
    {
        public Veth(string name, IAddress ipAddress)
        {
            Name = name;
            IpAddress = ipAddress;
        }

        private string Name { get; set; }
        private IAddress IpAddress{ get; set;}

        public void CleanUpVeth()
        {
            throw new NotImplementedException();
        }

        public void DeployVeth()
        {
            throw new NotImplementedException();
        }
    }
}
