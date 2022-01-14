using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using OverlayManagementService.Dtos;
using OverlayManagementService.Network;
using OverlayManagementService.Services;
using System.Collections.Generic;

namespace OverlayManagementService.Controllers
{
    /// <summary>
    /// This Controller provides endpoints for managing network and switches.
    /// </summary>
    [ApiController]
    [Route("network")]
    public class NetworkManagementController : ControllerBase
    {
        private readonly IOverlayNetworkManagementService _overlayNetworkManagementService;
        private readonly ILogger<NetworkManagementController> _logger;
        static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };

        /// <summary>
        /// Constructor for NetworkManagementController.
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="overlayNetworkManagementService">Overlay Network management service object</param>
        /// <returns>new NetworkManagementController object</returns>
        public NetworkManagementController(ILogger<NetworkManagementController> logger, IOverlayNetworkManagementService overlayNetworkManagementService)
        {
            _logger = logger;
            _overlayNetworkManagementService = overlayNetworkManagementService;
        }

        /// <summary>
        /// Endpoint that allows to get network by Virtual Network Identifier.
        /// </summary>
        /// <param name="vni">Virtual Network Identifier</param>
        /// <returns>IOverlayNetwork object</returns>
        [Authorize(Policy = "Admin")]
        [HttpGet("get/{vni}")]
        public IOverlayNetwork GetNetwork(string vni)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.QueryString.Value);
            return _overlayNetworkManagementService.GetNetworkByVni(vni);
        }

        /// <summary>
        /// Endpoint that allows to deploy the network.
        /// </summary>
        /// <param name="oVSConnection">OVSConnection DTO containing network information</param>
        /// <returns>IOverlayNetwork object</returns>
        [Authorize(Policy = "Admin")]
        [HttpPost("deploy")]
        public IOverlayNetwork DeployNetwork(OVSConnection oVSConnection)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.QueryString.Value);
            return _overlayNetworkManagementService.DeployNetwork(oVSConnection);

        }

        /// <summary>
        /// Endpoint that allows to delete the network.
        /// </summary>
        /// <param name="groupId">Group id to which network is assigned</param>
        [Authorize(Policy = "Admin")]
        [HttpDelete("delete/{groupId}")]
        public IActionResult DeleteNetwork(string groupId)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.QueryString.Value);
            _overlayNetworkManagementService.DeleteNetwork(groupId);
            return Ok();
        }


        /// <summary>
        /// Endpoint that allows to get the list of all deployed networks.
        /// </summary>
        /// <returns>IEnumerable of IOverlayNetwork objects</returns>
        [Authorize(Policy = "Admin")]
        [HttpGet("list")]
        public IEnumerable<IOverlayNetwork> GetOverlayNetworks()
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.QueryString.Value);
            return _overlayNetworkManagementService.GetAllNetworks();
        }

        //TODO
        /// <summary>
        /// Endpoint that allows to update the network. 
        /// </summary>
        /// <param name="overlayNetwork">IOverlayNetwork object containing updated information</param>
        /// <returns>updated IOverlayNetwork object</returns>
        [Authorize(Policy = "Admin")]
        [HttpPatch("update")]
        public IOverlayNetwork UpdateNetwork(IOverlayNetwork overlayNetwork)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.QueryString.Value);
            return _overlayNetworkManagementService.UpdateNetwork(overlayNetwork);
        }
    }
}
