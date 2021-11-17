
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


        public string GenerateUniqueVNI()
        {
            throw new NotImplementedException();
        }
    }
}
