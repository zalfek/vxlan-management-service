using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OverlayManagementClient.Models;
using OverlayManagementClient.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementClient.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVXLANManagementService _vXLANManagementService;

        public HomeController(ILogger<HomeController> logger, IVXLANManagementService vXLANManagementService)
        {
            _logger = logger;
            _vXLANManagementService = vXLANManagementService;
        }

        public IActionResult Index()
        {
            return View(_vXLANManagementService.GetNetworksAsync().Result);
        }

        public ActionResult Create()
        {
            return PartialView("_Create");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OVSConnection oVSConnection)
        {
            _vXLANManagementService.AddNetworkAsync(oVSConnection);
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Edit(string vni)
        {
            return PartialView("_NetworkDetails", _vXLANManagementService.GetNetworkAsync(vni).Result);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OverlayNetwork overlayNetwork)
        {
            _vXLANManagementService.EditNetworkAsync(overlayNetwork);
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(string groupId)
        {
            _vXLANManagementService.DeleteNetworkAsync(groupId);
            return Ok();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
