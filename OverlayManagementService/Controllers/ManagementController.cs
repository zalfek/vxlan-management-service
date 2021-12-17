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

    [ApiController]
    [Route("[controller]")]
    public class ManagementController : ControllerBase
    {
        private readonly IOverlayManagementService _vmOverlayManagementService;
        private readonly ILogger<ManagementController> _logger;
        static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };

        public ManagementController(ILogger<ManagementController> logger, IOverlayManagementService vmOverlayManagementService)
        {
            _logger = logger;
            _vmOverlayManagementService = vmOverlayManagementService;
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("deploy/machine")]
        public IOverlayNetwork DeployMachine(VmConnection vmConnection)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.QueryString.Value);
            return _vmOverlayManagementService.RegisterMachine(vmConnection);
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("get/network/{vni}")]
        public IOverlayNetwork GetNetwork(string vni)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.QueryString.Value);
            return _vmOverlayManagementService.GetNetworkByVni(vni);
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("get/switch/{key}")]
        public IOpenVirtualSwitch GetSwitch(string key)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.QueryString.Value);
            return _vmOverlayManagementService.GetSwitch(key);
        }


        [Authorize(Policy = "Admin")]
        [HttpPost("register/switch")]
        public IOpenVirtualSwitch RegisterSwitch(OpenVirtualSwitch openVirtualSwitch)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.QueryString.Value);
            return _vmOverlayManagementService.AddSwitch(openVirtualSwitch); ;
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("suspend/machine")]
        public IOverlayNetwork SuspendMachine(string groupId, Guid guid)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.QueryString.Value);
            return _vmOverlayManagementService.UnRegisterMachine(groupId, guid);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("deploy/network")]
        public IOverlayNetwork DeployNetwork(OVSConnection oVSConnection)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.QueryString.Value);
            return _vmOverlayManagementService.DeployNetwork(oVSConnection);

        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("delete/network/{groupId}")]
        public IActionResult DeleteNetwork(string groupId)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.QueryString.Value);
            _vmOverlayManagementService.DeleteNetwork(groupId);
            return Ok();
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("list/networks")]
        public IEnumerable<IOverlayNetwork> GetOverlayNetworks()
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.QueryString.Value);
            return _vmOverlayManagementService.GetAllNetworks();
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("list/switches")]
        public IEnumerable<IOpenVirtualSwitch> GetSwitches()
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.QueryString.Value);
            return _vmOverlayManagementService.GetAllSwitches();
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("update/network")]
        public IOverlayNetwork UpdateNetwork(IOverlayNetwork overlayNetwork)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _logger.LogInformation("Processing GET request: " + HttpContext.Request.QueryString.Value);
            return _vmOverlayManagementService.UpdateNetwork(overlayNetwork);
        }

    }
}
