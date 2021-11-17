using Microsoft.Extensions.Logging;
using OverlayManagementService.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public class Firewall : IFirewall
    {
        private readonly ILogger<Firewall> _logger;

        public Firewall(ILogger<Firewall> logger)
        {
            _logger = logger;
        }

        public void AddException(IUser user)
        {
            throw new NotImplementedException();
        }

        public void RemoveException(IUser user)
        {
            throw new NotImplementedException();
        }
    }
}
