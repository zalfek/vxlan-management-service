using Microsoft.Extensions.Logging;
using OverlayManagementService.DataTransferObjects;
using OverlayManagementService.Network;
using OverlayManagementService.Repositories;
using OverlayManagementService.Resolvers;
using System;
using System.Collections.Generic;


namespace OverlayManagementService.Services
{
    public class VMOverlayConnectionService : IOverlayConnectionService
    {

        private readonly IMembershipResolver _membershipResolver;
        private readonly IRepository _jsonRepository;
        private readonly ILogger<VMOverlayConnectionService> _logger;
        private readonly IFirewall _firewall;

        public VMOverlayConnectionService(IMembershipResolver membershipResolver, IRepository jsonRepository, ILogger<VMOverlayConnectionService> logger)
        {
            this._membershipResolver = membershipResolver;
            this._jsonRepository = jsonRepository;
            this._logger = logger;
        }

        public IOverlayNetwork GetOverlayNetwork(IMembership membership)
        {
            _jsonRepository.GetOverlayNetwork(membership.MembershipId);
            return null;
        }

        public List<IMembership> GetUserMemberships(IUser user)
        {
            _membershipResolver.GetUserMemberships(user);
            return null;
        }
    }
}
