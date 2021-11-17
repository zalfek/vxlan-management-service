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


        [HttpPost("register")]
        public IVirtualMachine RegisterMachine(IVmConnectionInfo vmConnectionInfo)
        {
            //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            _vmOverlayManagementService.RegisterMachine(vmConnectionInfo);
            return new VirtualMachine(new Guid(), HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(), new VmConnectionInfo());
        }


        [HttpPost("unregister")]
        public IVirtualMachine UnRegisterMachine(IVmConnectionInfo vmConnectionInfo)
        {
            //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            return new VirtualMachine(new Guid(), HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(), new VmConnectionInfo());
    }

    }
}
