using Microsoft.Extensions.Logging;
using OverlayManagementService.Network;
using OverlayManagementService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Services
{
    public class VMOverlayManagementService : IOverlayManagementService
    {

        private readonly ILogger<VMOverlayManagementService> _logger;
        private readonly IRepository jsonRepository;

        public IOverlayNetwork RegisterMachine(string data)
        {
            throw new NotImplementedException();
        }

        public IOverlayNetwork UnRegisterMachine(string data)
        {
            throw new NotImplementedException();
        }
    }
}
