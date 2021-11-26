using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using OverlayManagementService.DataTransferObjects;
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
        public IActionResult DeployMachine(VmConnectionInfo vmConnectionInfo)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _vmOverlayManagementService.RegisterMachine(vmConnectionInfo);
            return null;
        }


        [HttpPost("suspend/machine")]
        public IActionResult SuspendrMachine(VmConnectionInfo vmConnectionInfo)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _vmOverlayManagementService.UnRegisterMachine(vmConnectionInfo);
            return null;
        }


        [HttpPost("deploy/network")]
        public IOverlayNetwork DeployNetwork(VmConnectionInfo vmConnectionInfo)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            return _vmOverlayManagementService.DeployNetwork(vmConnectionInfo);

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
