using Microsoft.AspNetCore.Mvc;
using System;

namespace OverlayManagementService.DataTransferObjects
{
    public class Membership : IMembership
    {
        public Membership(string membershipId, string subject, Guid networkId)
        {
            MembershipId = membershipId;
            Subject = subject;
            NetworkId = networkId;
        }

        private string MembershipId { get; set; }
        private string Subject { get; set; }
        private Guid NetworkId { get; set; }

    }
}
