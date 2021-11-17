using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using OverlayManagementService.Network;
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

        private readonly ILogger<ManagementController> _logger;

        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };

        public ManagementController(ILogger<ManagementController> logger)
        {
            _logger = logger;
        }


        [HttpPost("register")]
        public IVirtualMachine RegisterMachine(IVirtualMachine model)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            return new VirtualMachine
            {
              Id = new Guid(),
              IPAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString()

            };
        }


        [HttpPost("unregister")]
        public IVirtualMachine UnRegisterMachine(IVirtualMachine model)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            return new VirtualMachine
            {
                Id = new Guid(),
                IPAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString()

            };
        }

    }
}
