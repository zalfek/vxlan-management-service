using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OverlayConnectionClient.Models;
using OverlayConnectionClient.Services;
using System.Diagnostics;


namespace OverlayConnectionClient.Controllers
{

    /// <summary>
    /// This Controller provides endpoints for users to manage their connections.
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVXLANConnectionService _vXLANConnectionService;

        /// <summary>
        /// Constructor for HomeController.
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="vXLANConnectionService">VXLAN Connection Service service object</param>
        /// <returns>new HomeController object</returns>
        public HomeController(ILogger<HomeController> logger, IVXLANConnectionService vXLANConnectionService)
        {
            _logger = logger;
            _vXLANConnectionService = vXLANConnectionService;
        }

        /// <summary>
        /// Endpoing to access homepage.
        /// </summary>
        public IActionResult Index()
        {
            return View(_vXLANConnectionService.GetAllNetworks());
        }

        /// <summary>
        /// Endpoint that allows to trigger the tunnel setup for client.
        /// </summary>
        /// <param name="groupId">Group id to witch network is assigned</param>
        [HttpPost]
        public IActionResult CreateConnection(string groupId)
        {
            try
            {
                _vXLANConnectionService.CreateConnection(groupId);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Endpoint that allows to suspend the client side tunnel.
        /// </summary>
        /// <param name="groupId">Group id to which network is assigned</param>
        public IActionResult Disconnect(string groupId)
        {
            try
            {
                _vXLANConnectionService.CleanUpConnection(groupId);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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
