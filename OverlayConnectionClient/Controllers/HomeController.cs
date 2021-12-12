using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OverlayConnectionClient.Models;
using OverlayConnectionClient.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayConnectionClient.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVXLANConnectionService _vXLANConnectionService;

        public HomeController(ILogger<HomeController> logger, IVXLANConnectionService vXLANConnectionService)
        {
            _logger = logger;
            _vXLANConnectionService = vXLANConnectionService;
        }

        public IActionResult Index()
        {
            return View(_vXLANConnectionService.GetAllNetworks());
        }

        [HttpPost]
        public IActionResult CreateConnection(string groupId)
        {
            _vXLANConnectionService.CreateConnection(groupId);
            return Ok();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
