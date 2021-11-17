using Microsoft.Extensions.Logging;
using OverlayManagementService.DataTransferObjects;
using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Repositories
{
    public class JsonRepository : IRepository
    {

        private readonly ILogger<JsonRepository> _logger;

        public IOverlayNetwork FindOverlayNetwork(IMembership membership)
        {
            throw new NotImplementedException();
        }

        public IOverlayNetwork GetOverlayNetwork(IMembership membership)
        {
            throw new NotImplementedException();
        }

        public IOverlayNetwork SaveOverlayNetwork(IMembership membership)
        {
            throw new NotImplementedException();
        }

        public IOverlayNetwork UpdateOverlayNetwork(IMembership membership)
        {
            throw new NotImplementedException();
        }
    }
}
