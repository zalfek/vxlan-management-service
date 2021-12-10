﻿using Microsoft.AspNetCore.Http;
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
            return View(_vXLANManagementService.GetNetworksAsync().Result);
        }

        public ActionResult Create()
        {
            return PartialView("_Create");
        }

        // POST: SwitchController/Create
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

        // GET: SwitchController/Edit/5
        public ActionResult Edit(string id)
        {
            return PartialView("_NetworkDetails", _vXLANManagementService.GetNetworkAsync(id).Result);
        }

        // POST: SwitchController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OverlayNetwork  overlayNetwork)
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