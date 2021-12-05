using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OverlayManagementClient.Models;
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
            var result = _vXLANManagementService.GetAsync().Result;
            return View(result);
        }

        public IActionResult NetworkDetails(string id)
        {
            return PartialView("_networkDetails", _vXLANManagementService.GetAsync(id).Result);
        }

    }
}
