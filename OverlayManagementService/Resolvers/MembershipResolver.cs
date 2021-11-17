using Microsoft.Extensions.Logging;
using OverlayManagementService.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OverlayManagementService.Resolvers
{
    public class MembershipResolver : IMembershipResolver
    {
        private readonly ILogger<MembershipResolver> _logger;

        public List<IMembership> GetUserMemberships(IUser user)
        {
            throw new NotImplementedException();
        }
    }
}
