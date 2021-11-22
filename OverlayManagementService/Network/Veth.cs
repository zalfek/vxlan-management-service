using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public class Veth : IVeth
    {
        public Veth(string name, string ipAddress)
        {
            Name = name;
            IpAddress = ipAddress;
        }

        public string Name { get; set; }
        public string IpAddress{ get; set;}

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
