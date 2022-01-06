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
    public class NetworkController : Controller
    {
        private readonly ILogger<NetworkController> _logger;
        private readonly IVXLANManagementService _vXLANManagementService;

        public NetworkController(ILogger<NetworkController> logger, IVXLANManagementService vXLANManagementService)
        {
            _logger = logger;
            _vXLANManagementService = vXLANManagementService;
        }

        [Authorize(Policy = "Admin")]
        public IActionResult Index()
        {
            return View(_vXLANManagementService.GetNetworks());
        }
        [Authorize(Policy = "Admin")]
        public ActionResult Create()
        {
            return PartialView("_Create");
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public ActionResult Create(OVSConnection oVSConnection)
        {
            _vXLANManagementService.AddNetwork(oVSConnection);
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Policy = "Admin")]
        public ActionResult NetworkDetails(string vni)
        {
            return PartialView("_NetworkDetails", _vXLANManagementService.GetNetwork(vni));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public ActionResult Edit(OverlayNetwork overlayNetwork) //TODO
        {
            _vXLANManagementService.EditNetwork(overlayNetwork);
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(string groupId)
        {
            _vXLANManagementService.DeleteNetwork(groupId);
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Policy = "Admin")]
        public ActionResult CreateTargetDevice()
        {
            return PartialView("_CreateTarget");
        }


        [Authorize(Policy = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTargetDevice(VmConnection vmConnection)
        {
            _vXLANManagementService.AddMachine(vmConnection);
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Policy = "Admin")]
        public ActionResult DeleteTargetDevice(Guid guid, string groupId)
        {
            _vXLANManagementService.RemoveMachine(groupId, guid);
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
