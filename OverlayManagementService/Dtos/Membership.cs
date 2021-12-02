using Microsoft.AspNetCore.Mvc;
using System;

namespace OverlayManagementService.Dtos
{
    public class Membership
    {
        public Membership(string membershipId, string subject, Guid networkId)
        {
            MembershipId = membershipId;
            Subject = subject;
            NetworkId = networkId;
        }

        public string MembershipId { get; set; }
        public string Subject { get; set; }
        public Guid NetworkId { get; set; }

    }
}
