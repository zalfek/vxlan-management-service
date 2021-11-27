
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public class VNI : IIdentifier
    {
        private readonly ILogger<VNI> _logger;
        private static List<string> VNIs;
        private static int PreviousVNI;

        public VNI()
        {
            VNIs = new List<string>();
            PreviousVNI = 0;
        }


        public string GenerateUniqueVNI()
        {
            PreviousVNI++;
            VNIs.Add(PreviousVNI.ToString());
            return PreviousVNI.ToString();
        }
    }
}
