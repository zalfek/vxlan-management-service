using OverlayManagementService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Resolvers
{
    public interface IMembershipResolver
    {

        public Task<List<String>> GetUserMemberships(Student user);

    }
}
