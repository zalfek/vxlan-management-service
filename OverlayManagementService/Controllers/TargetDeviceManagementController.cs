using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using Microsoft.Net.Http.Headers;
using OverlayManagementService.Dtos;
using OverlayManagementService.Network;
using OverlayManagementService.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OverlayManagementService.Controllers
{
    /// <summary>
    /// This Controller provides endpoints for managing taregt devices.
    /// </summary>
    [ApiController]
    [Route("device")]
    public class TargetDeviceManagementController : ControllerBase
    {
        private readonly ITargetDeviceManagementService _taregetDeviceManagementService;
        private readonly ILogger<TargetDeviceManagementController> _logger;
        static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };

        public TargetDeviceManagementController(ILogger<TargetDeviceManagementController> logger, ITargetDeviceManagementService taregetDeviceManagementService)
        {
            _logger = logger;
            _taregetDeviceManagementService = taregetDeviceManagementService;
        }

        /// <summary>
        /// Endpoint that allows to deploy device.
        /// </summary>
        /// <permission cref="Admin"><permission>
        /// <param name="vmConnection">VMConnection DTO</param>
        /// <returns>IOverlayNetwork object</returns>
        [Authorize(Policy = "Admin")]
        [HttpPost("deploy")]
        public IOverlayNetwork DeployMachine([FromForm] VmConnection vmConnection)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.QueryString.Value);
            return _taregetDeviceManagementService.RegisterMachine(vmConnection);
        }

        /// <summary>
        /// Endpoint that allows to remove the non-client device from network.
        /// </summary>
        /// <permission cref="Admin"><permission>
        /// <param name="groupId">Group id to which network is assigned</param>
        /// <param name="guid">Guid of the device</param>
        /// <returns>updated IOverlayNetwork object</returns>
        [Authorize(Policy = "Admin")]
        [HttpGet("suspend")]
        public IOverlayNetwork SuspendMachine(string groupId, Guid guid)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.QueryString.Value);
            return _taregetDeviceManagementService.UnRegisterMachine(groupId, guid);
        }
    }
}
