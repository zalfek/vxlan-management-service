using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using OverlayManagementService.Dtos;
using OverlayManagementService.Models;
using OverlayManagementService.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OverlayManagementService.Controllers
{

    /// <summary>
    /// This Controller provides endpoints for users to manage their connections.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ConnectionController : ControllerBase
    {
        private readonly ILogger<ConnectionController> _logger;
        private readonly IOverlayNetworkConnectionService _vmOverlayConnectionService;

        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };

        /// <summary>
        /// Constructor for ConnectionController.
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="vmOverlayConnectionService">Overlay Connection service object</param>
        /// <returns>new ConnectionController object</returns>
        public ConnectionController(ILogger<ConnectionController> logger, IOverlayNetworkConnectionService vmOverlayConnectionService)
        {
            _logger = logger;
            _vmOverlayConnectionService = vmOverlayConnectionService;
        }

        /// <summary>
        /// Endpoing for querying the list of neworks assigned to User.
        /// </summary>
        /// <returns>IEnumerable with ClientConnection</returns>
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

        /// <summary>
        /// Endpoint that allows to trigger the tunnel setup for client.
        /// </summary>
        /// <param name="groupId">Group id to witch network is assigned</param>
        /// <returns>ClientConnection</returns>
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

        /// <summary>
        /// Endpoint that allows to suspend the client side tunnel.
        /// </summary>
        /// <param name="groupId">Group id to which network is assigned</param>
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
