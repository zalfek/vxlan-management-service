using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class SwitchController : Controller
    {


        private readonly ILogger<SwitchController> _logger;
        private readonly IVXLANManagementService _vXLANManagementService;

        public SwitchController(ILogger<SwitchController> logger, IVXLANManagementService vXLANManagementService)
        {
            _logger = logger;
            _vXLANManagementService = vXLANManagementService;
        }



        [Authorize(Policy = "Admin")]
        public ActionResult Index()
        {
            return View(_vXLANManagementService.GetSwitchesAsync().Result);
        }

        [Authorize(Policy = "Admin")]
        public ActionResult Details(string key)
        {
            return PartialView("_SwitchDetails", _vXLANManagementService.GetSwitchAsync(key).Result);
        }

        [Authorize(Policy = "Admin")]
        public ActionResult Create()
        {
            return PartialView("_Create");
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OpenVirtualSwitch openVirtualSwitch)
        {

            _vXLANManagementService.AddSwitchAsync(openVirtualSwitch);
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
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
        public ActionResult Delete(int id)
        {
            return View();
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
