
using Microsoft.Extensions.Logging;


namespace OverlayManagementService.Network
{
    public class Bridge: IBridge
    {
        private readonly ILogger<Bridge> _logger;

        public Bridge(ILogger<Bridge> logger, string name, string switchPort, IVeth virtualInterface)
        {
            _logger = logger;
            Name = name;
            SwitchPort = switchPort;
            VirtualInterface = virtualInterface;
        }

        private string Name { get; set; }
        private string SwitchPort { get; set; }
        private IVeth VirtualInterface { get; set; }

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
