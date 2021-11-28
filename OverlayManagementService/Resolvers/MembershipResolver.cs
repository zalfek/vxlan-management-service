using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using OverlayManagementService.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace OverlayManagementService.Resolvers
{
    public class MembershipResolver : IMembershipResolver
    {
        private readonly ILogger<MembershipResolver> _logger;
        private readonly GraphServiceClient _graphServiceClient;

        public MembershipResolver(GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }


        public async Task<List<String>> GetUserMemberships(Student user)
        {
            var groupIds = new List<String>()
            {
                "groupIds-value"
            };

            await _graphServiceClient.Groups["{group-id}"]
                .CheckMemberGroups(groupIds)
                .Request()
                .PostAsync();
            Console.WriteLine(groupIds);

            return groupIds;
        }
    }
}
