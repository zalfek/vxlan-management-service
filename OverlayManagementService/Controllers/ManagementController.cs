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

        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
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
            return _vmOverlayManagementService.RegisterMachine(vmConnection);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("register/switch")]
        public IOpenVirtualSwitch RegisterSwitch(OpenVirtualSwitch openVirtualSwitch)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            return _vmOverlayManagementService.AddSwitch(openVirtualSwitch); ;
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("suspend/machine")]
        public IOverlayNetwork SuspendMachine(VmConnection vmConnection)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            return _vmOverlayManagementService.UnRegisterMachine(vmConnection);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("deploy/network")]
        public IOverlayNetwork DeployNetwork(OVSConnection oVSConnection)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            return _vmOverlayManagementService.DeployNetwork(oVSConnection);

        }

        [Authorize(Policy = "Admin")]
        [HttpPost("suspend/network")]
        public IActionResult SuspendNetwork(Membership membership)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _vmOverlayManagementService.SuspendNetwork(membership);
            return null;
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("delete/network/{id}")]
        public IActionResult DeleteNetwork(int id)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            //_vmOverlayManagementService.DeleteNetwork(membership);

            return Ok(id);
        }


        [Authorize(Policy = "Admin")]
        [HttpGet("get/token")]
        public string GetToken()
        {
            return HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        }

            
        [HttpGet("list/networks")]
        public IEnumerable<IOverlayNetwork> GetOverlayNetworks()
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            return _vmOverlayManagementService.GetAllNetworks();
        }

    }
}
