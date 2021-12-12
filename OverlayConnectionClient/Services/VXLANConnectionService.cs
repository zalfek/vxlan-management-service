using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using OverlayConnectionClient.Models;
using Microsoft.Extensions.Logging;
using OverlayConnectionClient.Repositories;
using OverlayConnectionClient.Network;

namespace OverlayConnectionClient.Services
{
    public class VXLANConnectionService : IVXLANConnectionService
    {
        private readonly ILogger<VXLANConnectionService> _logger;
        private readonly INetworkRepository _repository;
        private readonly IRepository _jsonRepository;
        private readonly IDictionary<Guid, ILinuxVXLANInterface> _linuxVXLANInterfaces;

        public VXLANConnectionService(ILogger<VXLANConnectionService> logger, INetworkRepository repository, IRepository jsonRepository)
        {
            _logger = logger;
            _repository = repository;
            _linuxVXLANInterfaces = new Dictionary<Guid, ILinuxVXLANInterface>();
            _jsonRepository = jsonRepository;
        }

        public void CreateConnection(string groupId)
        {
            _logger.LogInformation("Searching for network with id: " + groupId);
            OverlayNetwork overlayNetwork =  _repository.GetNetworkAsync(groupId).Result;
            _logger.LogInformation("Network found: " + overlayNetwork.ToString());
            _logger.LogInformation("Setting up interface");
            ILinuxVXLANInterface linuxVXLANInterface = new LinuxVXLANInterface("vxlan" + _linuxVXLANInterfaces.Count(), overlayNetwork.VNI, "4789", overlayNetwork.RemoteIp, overlayNetwork.LocalIp);
            _logger.LogInformation("New interface created: " + linuxVXLANInterface.ToString());
            _jsonRepository.SaveInterface(groupId, linuxVXLANInterface);
            _logger.LogInformation("Initiating interface deployment");
            //linuxVXLANInterface.DeployInterface();
        }

        public void CleanUpConnection(string groupId)
        {
            _logger.LogInformation("Initiating interface cleanup");
            ILinuxVXLANInterface linuxVXLANInterface = _jsonRepository.GetVXLANInterface(groupId);
            //linuxVXLANInterface.CleanUpInterface();
            _jsonRepository.DeleteInterface(groupId);
        }

        public IEnumerable<OverlayNetwork> GetAllNetworks()
        {
            _logger.LogInformation("Requesting for networks from repository");
            return _repository.GetNetworksAsync().Result;
        }
    }
}
