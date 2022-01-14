using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OverlayManagementService.Network;
using OverlayManagementService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OverlayManagementService.Services
{

    /// <summary>
    /// This Service is running in background and removes all clent connections at midnight.
    /// </summary>
    public class ClientsRemovalService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<ClientsRemovalService> _logger;
        private Timer _timer;
        private readonly INetworkRepository _networkRepository;
        private readonly IFirewallRepository _firewallRepository;

        /// <summary>
        /// Constructor for ClientsRemovalService.
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="networkRepository">Network repository object</param>
        /// <param name="firewallRepository">firewall repository object</param>
        /// <returns>new ClientsRemovalService object</returns>
        public ClientsRemovalService(ILogger<ClientsRemovalService> logger, INetworkRepository networkRepository, IFirewallRepository firewallRepository)
        {
            _logger = logger;
            _networkRepository = networkRepository;
            _firewallRepository = firewallRepository;
        }

        /// <summary>
        /// Method that sets the timer for periodic removal of client connections.
        /// </summary>
        /// <param name="stoppingToken">Cancellation Token</param>
        /// <returns>Task execution result</returns>
        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");
            int hourSpan = 24 - DateTime.Now.Hour;
            _timer = new Timer(RemoveClientConnection, null, TimeSpan.FromHours(hourSpan),
                TimeSpan.FromHours(24));
            return Task.CompletedTask;
        }

        private void RemoveClientConnection(object state)
        {
            var count = Interlocked.Increment(ref executionCount);
            _logger.LogInformation(
           "Timed Hosted Service is working. Count: {Count}", count);
            _logger.LogInformation(
           "Initiating daily client removal");
            IDictionary<string, IOverlayNetwork> networks = _networkRepository.GetAllNetworks();
            foreach (KeyValuePair<string, IOverlayNetwork> keyValuePair in networks)
            {
                _logger.LogInformation("Removing clients from network with id: " + keyValuePair.Value.GroupId);
                IFirewall firewall = _firewallRepository.GetFirewall(keyValuePair.Value.OpenVirtualSwitch.Key);
                for (int i = keyValuePair.Value.Clients.Count - 1; i > -1; --i)
                {
                    _logger.LogInformation("Removing firewall exception for " + keyValuePair.Value.Clients[i]);
                    firewall.RemoveException(keyValuePair.Value.Clients[i].IpAddress);
                    _logger.LogInformation("Removing from network client with ip: " + keyValuePair.Value.Clients[i]);
                    keyValuePair.Value.RemoveClient(keyValuePair.Value.Clients[i]);
                }
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
