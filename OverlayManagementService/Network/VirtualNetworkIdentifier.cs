
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{

    /// <summary>
    /// CLass which encapsulates logic for generating an unique VNI.
    /// </summary>
    public class VirtualNetworkIdentifier : IIdentifier
    {
        private readonly ILogger<IIdentifier> _logger;
        private static List<string> VNIs;
        private static int PreviousVNI;

        public VirtualNetworkIdentifier()
        {
            _logger = LoggerFactory.Create(logging => logging.AddConsole()).CreateLogger<IIdentifier>();
            VNIs = new List<string>();
            PreviousVNI = 0;
        }

        /// <summary>
        /// Method Generates unique VNI.
        /// </summary>
        public string GenerateUniqueVNI()
        {
            PreviousVNI++;
            _logger.LogInformation("New VNI was generated:" + PreviousVNI.ToString());
            VNIs.Add(PreviousVNI.ToString());
            return PreviousVNI.ToString();
        }
    }
}
