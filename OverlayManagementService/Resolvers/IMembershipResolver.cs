using OverlayManagementService.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Resolvers
{
    public interface IMembershipResolver
    {

        public List<Membership> GetAllMemberships(User user); 

    }
}
