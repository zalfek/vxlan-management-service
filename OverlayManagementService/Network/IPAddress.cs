using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    /// <summary>
    /// CLass which encapsulates logic for generating an unique IPV4 Address.
    /// </summary>
    public class IPAddress : IAddress
    {
        private readonly ILogger<IPAddress> _logger;
        private List<string> IPs;
        private List<int> LastIPV4;

        public IPAddress()
        {
            _logger = new LoggerFactory().CreateLogger<IPAddress>();
            LastIPV4 = new List<int>();
            IPs = new List<string>();
            LastIPV4.Add(192);
            LastIPV4.Add(168);
            LastIPV4.Add(5);
            LastIPV4.Add(3);
    }

        /// <summary>
        /// Method Generates an IPV4 address which will be unique within the network with specific VNI.
        /// </summary>
        public string GenerarteUniqueIPV4Address()
        {
            _logger.LogInformation("Generating new IP address");
            LastIPV4[3] = ++LastIPV4[3];
            IPs.Add(String.Join(".", LastIPV4.ToArray()));
            _logger.LogInformation("New IP is " + String.Join(".", LastIPV4.ToArray()));
            return String.Join(".", LastIPV4.ToArray());
        }
    }
}
