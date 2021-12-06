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



        // GET: SwitchController
        public ActionResult Index()
        {
            return View(_vXLANManagementService.GetSwitchesAsync().Result);
        }

        // GET: SwitchController/Details/5
        public ActionResult Details(string key)
        {
            return PartialView("_SwitchDetails", _vXLANManagementService.GetSwitchAsync(key).Result);
        }

        // GET: SwitchController/Create
        public ActionResult Create()
        {
            return PartialView("_Create");
        }

        // POST: SwitchController/Create
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

        // GET: SwitchController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SwitchController/Edit/5
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

        // GET: SwitchController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SwitchController/Delete/5
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
