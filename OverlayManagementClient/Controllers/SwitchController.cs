using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OverlayManagementClient.Models;
using OverlayManagementClient.Services;

namespace OverlayManagementClient.Controllers
{
    /// <summary>
    /// This Controller provides endpoints for admins to manage switches.
    /// </summary>
    [Authorize]
    public class SwitchController : Controller
    {

        private readonly ILogger<SwitchController> _logger;
        private readonly IVXLANManagementService _vXLANManagementService;

        public SwitchController(ILogger<SwitchController> logger, IVXLANManagementService vXLANManagementService)
        {
            _logger = logger;
            _vXLANManagementService = vXLANManagementService;
        }


        /// <summary>
        /// Endpoing to access list of Switches.
        /// </summary>
        [Authorize(Policy = "Admin")]
        public ActionResult Index()
        {
            return View(_vXLANManagementService.GetSwitches());
        }

        /// <summary>
        /// Endpoint for getting the switch details window.
        /// </summary>
        /// <param name="key">Open virtual switch prefix</param>
        [Authorize(Policy = "Admin")]
        public ActionResult Details(string key)
        {
            return PartialView("_SwitchDetails", _vXLANManagementService.GetSwitch(key));
        }

        /// <summary>
        /// Endpoint that for getting the form to register the switch.
        /// </summary>
        [Authorize(Policy = "Admin")]
        public ActionResult Create()
        {
            return PartialView("_Create");
        }

        /// <summary>
        /// Endpoint to submit form data for switch registration.
        /// </summary>
        /// <param name="ovsRegistration">OvsRegistration DTO containing all information for switch registration</param>
        [Authorize(Policy = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OvsRegistration ovsRegistration)
        {
            try
            {
                _vXLANManagementService.AddSwitch(ovsRegistration);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Endpoint to remove the unregister the switch.
        /// </summary>
        /// <param name="groupId">Open virtual switch prefix</param>
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(string key)
        {
            try
            {
                _vXLANManagementService.DeleteSwitch(key);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
