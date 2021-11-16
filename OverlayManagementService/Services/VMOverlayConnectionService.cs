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

        public VMOverlayConnectionService(IMembershipResolver membershipResolver, IRepository jsonRepository)
        {
            this.membershipResolver = membershipResolver;
            this.jsonRepository = jsonRepository;
        }

        public VXLANOverlayNetwork GetOverlayNetwork(Membership membership)
        {
            jsonRepository.GetOverlayNetwork(membership);
            return null;
        }

        public List<Membership> GetAllMemberships(User user)
        {
            membershipResolver.GetAllMemberships(user);
            return null;
        }


    }
}
