using Microsoft.Extensions.Logging;
using OverlayManagementService.Dtos;
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
        private readonly IRepository _jsonRepository;

        public IOverlayNetwork RegisterMachine(IVmConnectionInfo vmConnectionInfo)
        {
            throw new NotImplementedException();
        }

        public IOverlayNetwork UnRegisterMachine(IVmConnectionInfo vmConnectionInfo)
        {
            throw new NotImplementedException();
        }
    }
}
