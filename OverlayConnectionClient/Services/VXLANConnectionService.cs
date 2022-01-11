using System.Collections.Generic;
using OverlayConnectionClient.Models;
using Microsoft.Extensions.Logging;
using OverlayConnectionClient.Repositories;
using OverlayConnectionClient.Network;
using OverlayConnectionClient.Factories;

namespace OverlayConnectionClient.Services
{
    /// <summary>
    /// Service class(Facade) which acts as interface for all Network connection related operations.
    /// </summary>
    public class VXLANConnectionService : IVXLANConnectionService
    {
        private readonly ILogger<VXLANConnectionService> _logger;
        private readonly INetworkRepository _networkRepository;
        private readonly IInterfaceRepository _interfaceRepository;
        private readonly ILinuxVxlanInterfaceFactory _linuxVxlanInterfaceFactory;


        /// <summary>
        /// Constructor for OverlayNetworkConnectionService.
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="networkRepository">Network repository object</param>
        /// <param name="linuxVxlanInterfaceFactory">Factory object which will be responsible for creation of VXLAN Interfaces</param>
        /// <param name="interfaceRepository">Interface repository object</param>
        /// <returns>new OverlayNetworkConnectionService object</returns>
        public VXLANConnectionService(
            ILogger<VXLANConnectionService> logger,
            INetworkRepository networkRepository,
            IInterfaceRepository interfaceRepository,
            ILinuxVxlanInterfaceFactory linuxVxlanInterfaceFactory
            )
        {
            _logger = logger;
            _networkRepository = networkRepository;
            _interfaceRepository = interfaceRepository;
            _linuxVxlanInterfaceFactory = linuxVxlanInterfaceFactory;
        }

        /// <summary>
        /// Method that initiates client side connection deployment.
        /// </summary>
        /// <param name="groupId">Group id to which network is assigned</param>
        public void CreateConnection(string groupId)
        {
            _logger.LogInformation("Searching for network with id: " + groupId);
            OverlayNetwork overlayNetwork = _networkRepository.GetNetworkAsync(groupId).Result;
            _logger.LogInformation("Network found: " + overlayNetwork.ToString());
            _logger.LogInformation("Setting up interface");
            ILinuxVXLANInterface linuxVXLANInterface = _linuxVxlanInterfaceFactory.CreateInterface(overlayNetwork.VNI, overlayNetwork.RemoteIp, overlayNetwork.LocalIp);
            _logger.LogInformation("New interface created wi remote IP: " + linuxVXLANInterface.DstIP);
            _interfaceRepository.SaveInterface(groupId, linuxVXLANInterface);
            _logger.LogInformation("Initiating interface deployment");
            linuxVXLANInterface.DeployInterface();
        }

        /// <summary>
        /// Method initiates cleanup of the client side connection.
        /// </summary>
        /// <param name="groupId">Group id to which network is assigned</param>
        public void CleanUpConnection(string groupId)
        {
            _logger.LogInformation("Initiating interface cleanup");
            ILinuxVXLANInterface linuxVXLANInterface = _interfaceRepository.GetVXLANInterface(groupId);
            linuxVXLANInterface.CleanUpInterface();
            _interfaceRepository.DeleteInterface(groupId);
            try
            {
                _networkRepository.RemoveClientAsync(groupId);
            }
            catch (System.Exception)
            {
                throw;
            }
            
        }

        /// <summary>
        /// Method that queries for assigned networks.
        /// </summary>
        /// <returns>IEnumerable of OverlayNetwork objects</returns>
        public IEnumerable<OverlayNetwork> GetAllNetworks()
        {
            _logger.LogInformation("Requesting for networks from repository");
            return _networkRepository.GetNetworksAsync().Result;
        }
    }
}
