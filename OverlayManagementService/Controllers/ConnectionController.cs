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
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using OverlayManagementService.Models;

namespace OverlayManagementService.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ConnectionController : ControllerBase
    {
        private readonly ILogger<ConnectionController> _logger;
        private readonly IOverlayConnectionService _vmOverlayConnectionService;
        private readonly IHttpContextAccessor _accessor;

        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };

        public ConnectionController(ILogger<ConnectionController> logger, IOverlayConnectionService vmOverlayConnectionService, IHttpContextAccessor accessor)
        {
            _logger = logger;
            _vmOverlayConnectionService = vmOverlayConnectionService;
            _accessor = accessor;
        }

        [Authorize(Policy = "Member")]
        [HttpGet("list/networks")]
        public IEnumerable<ClientConnection> GetAllNetworks()
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.QueryString.Value);
            _logger.LogInformation("Extracting user groups ");
            IEnumerable<Claim> groups = User.Claims.Where(claim => claim.Type == "groups");
            _logger.LogInformation("User belongs to " + groups.ToString());
            _logger.LogInformation("Forwarding request to " + _vmOverlayConnectionService.GetType().ToString());
            return _vmOverlayConnectionService.GetAllNetworks(groups);
        }


        [Authorize(Policy = "Member")]
        [HttpGet("get/network/{groupId}")]
        public ClientConnection CreateConnection(string groupId)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            Student user = new()
            { 
            Name = HttpContext.User.GetDisplayName(),
            IpAddress = "192.168.40.4" //_accessor.HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4().ToString()
            };
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.Path);
            return _vmOverlayConnectionService.CreateConnection(groupId, user);
        }

        [Authorize(Policy = "Member")]
        [HttpGet("suspend/connection/{groupId}")]
        public void SuspendConnection(string groupId)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            Student user = new()
            {
                Name = HttpContext.User.GetDisplayName(),
                IpAddress = "192.168.40.4" //_accessor.HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4().ToString()
            };
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.Path);
            _vmOverlayConnectionService.SuspendConnection(groupId, user);
        }

    }
}
