using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OverlayManagementClient.Models;
using OverlayManagementClient.Services;
using System;
using System.Diagnostics;

namespace OverlayManagementClient.Controllers
{
    /// <summary>
    /// This Controller provides endpoints for admins to manage Networks.
    /// </summary>
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

        /// <summary>
        /// Endpoing to access homepage(List of networks).
        /// </summary>
        [Authorize(Policy = "Admin")]
        public IActionResult Index()
        {
            return View(_vXLANManagementService.GetNetworks());
        }

        /// <summary>
        /// Endpoint that for getting the form for deployment of network.
        /// </summary>
        [Authorize(Policy = "Admin")]
        public ActionResult Create()
        {
            return PartialView("_Create");
        }

        /// <summary>
        /// Endpoint to submit form data for network deployment
        /// </summary>
        /// <param name="oVSConnection">OVSConnection DTO containing all information for network deployment</param>
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

        /// <summary>
        /// Endpoint for getting the Network details window.
        /// </summary>
        /// <param name="vni">VNI of the network for which details window should be returned</param>
        [Authorize(Policy = "Admin")]
        public ActionResult NetworkDetails(string vni)
        {
            return PartialView("_NetworkDetails", _vXLANManagementService.GetNetwork(vni));
        }

        /// <summary>
        /// !!!TODO not implemented on frontend and not tested!!!
        /// Endpoint to edit the existing network.
        /// </summary>
        /// <param name="overlayNetwork">updated OverlayNetwork object</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public ActionResult Edit(OverlayNetwork overlayNetwork) //TODO
        {
            try
            {
                _vXLANManagementService.EditNetwork(overlayNetwork);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Endpoint to remove the existing network.
        /// </summary>
        /// <param name="groupId">Group id to witch network is assigned</param>
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(string groupId)
        {
            try
            {
                _vXLANManagementService.DeleteNetwork(groupId);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Endpoint that for getting the form for deployment of taregt device.
        /// </summary>
        [Authorize(Policy = "Admin")]
        public ActionResult CreateTargetDevice()
        {
            return PartialView("_CreateTarget");
        }

        /// <summary>
        /// Endpoint to submit form data for target device deployment
        /// </summary>
        /// <param name="vmConnection">VmConnection DTO containing all information for device deployment</param>
        [Authorize(Policy = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTargetDevice(VmConnection vmConnection)
        {
            try
            {
                _vXLANManagementService.AddTargetDevice(vmConnection);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Endpoint to remove target device from the network.
        /// </summary>
        /// <param name="guid">guid of the device</param>
        /// <param name="groupId">Group id to witch network is assigned</param>
        [Authorize(Policy = "Admin")]
        public ActionResult DeleteTargetDevice(Guid guid, string groupId)
        {
            try
            {
                _vXLANManagementService.RemoveTargetDevice(groupId, guid);
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
