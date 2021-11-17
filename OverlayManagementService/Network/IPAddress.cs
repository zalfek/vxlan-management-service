using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public class IPAddress : IAddress
    {
        private readonly ILogger<IPAddress> _logger;
        private readonly string IPV4;
        private readonly string IPV6;
        public void GenerarteUniqueIPV4Address()
        {
            throw new NotImplementedException();
        }
    }
}
