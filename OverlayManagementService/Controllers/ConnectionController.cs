using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using OverlayManagementService.Network;
using OverlayManagementService.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;


namespace OverlayManagementService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ConnectionController : ControllerBase
    {
        private readonly ILogger<ConnectionController> _logger;

        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };

        public ConnectionController(ILogger<ConnectionController> logger)
        {
            _logger = logger;
        }


        [HttpGet("memberships")]
        public IEnumerable<Membership> GetAllMemberships(User user)
        {
            //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Membership(index.ToString(),
                "DWSYS",
                Guid.NewGuid()
            ))
            .ToArray();
        }



        [HttpGet("{membershipid}")]
        public IOverlayNetwork GetConnectionInfo()
        {
            //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            return new VXLANOverlayNetwork
            {
                DestIpAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(),
                Port = "4891",
                VNI = "1000",
                Guid = Guid.NewGuid()
        };
        }


    }
}
