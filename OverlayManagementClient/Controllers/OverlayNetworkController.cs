using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OverlayManagementClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementClient.Controllers
{
    public class OverlayNetworkController : Controller
    {
        private readonly ILogger<OverlayNetworkController> _logger;
        private readonly IVXLANManagementService _vXLANManagementService;

        public OverlayNetworkController(ILogger<OverlayNetworkController> logger, IVXLANManagementService vXLANManagementService)
        {
            _logger = logger;
            _vXLANManagementService = vXLANManagementService;
        }

        public IActionResult Index()
        {
            return View(_vXLANManagementService.GetAsync().Result);
        }
    }
}
