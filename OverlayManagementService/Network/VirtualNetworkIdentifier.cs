
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public class VirtualNetworkIdentifier : IIdentifier
    {
        private readonly ILogger<VirtualNetworkIdentifier> _logger;
        private static List<string> VNIs;
        private static int PreviousVNI;

        public VirtualNetworkIdentifier()
        {
            _logger = new LoggerFactory().CreateLogger<VirtualNetworkIdentifier>();
            VNIs = new List<string>();
            PreviousVNI = 0;
        }


        public string GenerateUniqueVNI()
        {
            PreviousVNI++;
            _logger.LogInformation("New VNI was generated:" + PreviousVNI.ToString());
            VNIs.Add(PreviousVNI.ToString());
            return PreviousVNI.ToString();
        }
    }
}
