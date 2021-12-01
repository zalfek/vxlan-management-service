using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using OverlayManagementService.Network;
using OverlayManagementService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using OverlayManagementService.Services;
using Microsoft.Identity.Web;

namespace OverlayManagementService.Controllers
{
    [Authorize(Roles = "2a0e3b19-4ec1-42f1-b049-ffd57d75996f")]
    [ApiController]
    [Route("[controller]")]
    public class ConnectionController : ControllerBase
    {
        private readonly ILogger<ConnectionController> _logger;
        private readonly IOverlayConnectionService _vmOverlayConnectionService;

        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };

        public ConnectionController(ILogger<ConnectionController> logger, IOverlayConnectionService vmOverlayConnectionService)
        {
            _logger = logger;
            _vmOverlayConnectionService = vmOverlayConnectionService;
        }


        [HttpGet("memberships")]
        [AuthorizeForScopes(Scopes = new[] { "user.read" })]
        public IEnumerable<Membership> GetAllMemberships()
        {

            Student user = new Student(
                "John",
                "Doe",
                "john.doe@hs-ulm.de",
                "537faa0c-9461-4be0-85cb-87fcb4105881",
               "255.255.255.255"
               );
            //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.ToString());
            return _vmOverlayConnectionService.GetUserMemberships(user);
        }



        [HttpGet("connection")]
        public IOverlayNetwork GetConnectionInfo([FromQuery] Membership membership)
        {
            //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.ToString());
            return _vmOverlayConnectionService.GetOverlayNetwork(membership);
        }


    }
}
