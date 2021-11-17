using Microsoft.Extensions.Logging;
using OverlayManagementService.DataTransferObjects;
using OverlayManagementService.Network;
using OverlayManagementService.Repositories;
using OverlayManagementService.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Services
{
    public class VMOverlayConnectionService : IOverlayConnectionService
    {

        private readonly IMembershipResolver membershipResolver;
        private readonly IRepository jsonRepository;
        private readonly ILogger<VMOverlayConnectionService> _logger;

        public VMOverlayConnectionService(IMembershipResolver membershipResolver, IRepository jsonRepository, ILogger<VMOverlayConnectionService> logger)
        {
            this.membershipResolver = membershipResolver;
            this.jsonRepository = jsonRepository;
            this._logger = logger;
        }

        public IOverlayNetwork GetOverlayNetwork(IMembership membership)
        {
            jsonRepository.GetOverlayNetwork(membership);
            return null;
        }

        public List<IMembership> GetUserMemberships(IUser user)
        {
            membershipResolver.GetUserMemberships(user);
            return null;
        }
    }
}
