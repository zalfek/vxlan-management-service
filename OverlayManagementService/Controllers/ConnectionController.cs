﻿using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Policy = "Member")]
        [HttpGet("memberships")]
        [AuthorizeForScopes(Scopes = new[] { "user.read" })]
        public IEnumerable<Membership> GetAllMemberships()
        {
            return null;
        }


        [Authorize(Policy = "Member")]
        [HttpGet("connection")]
        public IOverlayNetwork GetConnectionInfo([FromQuery] Membership membership)
        {
            //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.ToString());
            return _vmOverlayConnectionService.GetOverlayNetwork(membership);
        }


    }
}
