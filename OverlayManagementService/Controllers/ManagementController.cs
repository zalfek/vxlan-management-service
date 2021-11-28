using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using OverlayManagementService.Dtos;
using OverlayManagementService.Network;
using OverlayManagementService.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OverlayManagementService.Controllers
{
    [Authorize]
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


        [HttpPost("deploy/machine")]
        public IActionResult DeployMachine(VmConnection vmConnection)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _vmOverlayManagementService.RegisterMachine(vmConnection);
            return null;
        }


        [HttpPost("suspend/machine")]
        public IActionResult SuspendrMachine(VmConnection vmConnection)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _vmOverlayManagementService.UnRegisterMachine(vmConnection);
            return null;
        }


        [HttpPost("deploy/network")]
        public IOverlayNetwork DeployNetwork(OVSConnection oVSConnection)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            return _vmOverlayManagementService.DeployNetwork(oVSConnection);

        }


        [HttpPost("suspend/network")]
        public IActionResult SuspendNetwork(Membership membership)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _vmOverlayManagementService.SuspendNetwork(membership);
            return null;
        }


        [HttpDelete("delete/network/{id}")]
        public IActionResult DeleteNetwork(int id)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            //_vmOverlayManagementService.DeleteNetwork(membership);

            return Ok(id);
        }

    }
}
