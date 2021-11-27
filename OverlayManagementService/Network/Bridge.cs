
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace OverlayManagementService.Network
{
    public class Bridge: IBridge
    {
        private readonly ILogger<IBridge> _logger;

        public Bridge(string name, IVeth virtualInterface, string vni)
        {
            _logger = new LoggerFactory().CreateLogger<IBridge>();
            Name = name;
            VirtualInterface = virtualInterface;
            VNI = vni;
        }

        public string VNI { get; set; }
        public string Name { get; set; }
        public IVeth VirtualInterface { get; set; }
        public List<IVXLANInterface> VXLANInterfaces { get; set; }

        public void DeployVXLANInterface(IVirtualMachine virtualMachine)
        {
            IVXLANInterface vXLANInterface = new VXLANInterface(Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 14),
                "vxlan",
                virtualMachine.IPAddress,
                VNI,
                this
                );
            vXLANInterface.DeployVXLANInterface();
            VXLANInterfaces.Add(vXLANInterface);
        }

        public void CleanUpBridge()
        {
            VirtualInterface.CleanUpVeth();
            throw new System.NotImplementedException();
        }

        public void DeployBridge()
        {
            VirtualInterface.DeployVeth();
            throw new System.NotImplementedException();
        }
    }

}
