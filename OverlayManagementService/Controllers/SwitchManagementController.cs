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
    /// This Controller provides endpoints for managing network and switches.
    /// </summary>
    [ApiController]
    [Route("switch")]
    public class SwitchManagementController : ControllerBase
    {
        private readonly ISwitchManagementService _switchManagementService;
        private readonly ILogger<SwitchManagementController> _logger;
        static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };

        /// <summary>
        /// Constructor for SwitchManagementController.
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="switchManagementService">Switch management service object</param>
        /// <returns>new SwitchManagementController object</returns>
        public SwitchManagementController(ILogger<SwitchManagementController> logger, ISwitchManagementService switchManagementService)
        {
            _logger = logger;
            _switchManagementService = switchManagementService;
        }

        /// <summary>
        /// Endpoint that allows to get switch by switch key.
        /// </summary>
        /// <param name="key">Key that that identifies the switch f.e thu</param>
        /// <returns>IOpenVirtuaSwitch object</returns>
        [Authorize(Policy = "Admin")]
        [HttpGet("get/{key}")]
        public IOpenVirtualSwitch GetSwitch(string key)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.QueryString.Value);
            return _switchManagementService.GetSwitch(key);
        }

        /// <summary>
        /// Endpoint that allows to register the switch.
        /// </summary>
        /// <param name="ovsRegistration">OvsRegistration DTO containing switch information</param>
        /// <returns>IOpenVirtuaSwitch object</returns>
        [Authorize(Policy = "Admin")]
        [HttpPost("register")]
        public IOpenVirtualSwitch RegisterSwitch([FromForm] OvsRegistration ovsRegistration)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.QueryString.Value);
            return _switchManagementService.AddSwitch(ovsRegistration); ;
        }

        /// <summary>
        /// Endpoint that allows to get the list of all deployed switches.
        /// </summary>
        /// <returns>IEnumerable of IOpenVirtualSwitch objects</returns>
        [Authorize(Policy = "Admin")]
        [HttpGet("list")]
        public IEnumerable<IOpenVirtualSwitch> GetSwitches()
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.QueryString.Value);
            return _switchManagementService.GetAllSwitches();
        }

         /// <summary>
        /// Endpoint that allows to delete the switch. 
        /// </summary>
        /// <param name="key">Key that that identifies the switch f.e thu</param>
        [Authorize(Policy = "Admin")]
        [HttpDelete("delete/{key}")]
        public IActionResult DeleteSwitch(string key)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.QueryString.Value);
            _switchManagementService.RemoveSwitch(key);
            return Ok();
        }

    }
}
