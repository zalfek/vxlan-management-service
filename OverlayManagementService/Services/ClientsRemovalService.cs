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
    public class ClientsRemovalService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<ClientsRemovalService> _logger;
        private Timer _timer;
        private readonly INetworkRepository _networkRepository;
        private readonly IFirewallRepository _firewallRepository;

        public ClientsRemovalService(ILogger<ClientsRemovalService> logger, INetworkRepository networkRepository)
        {
            _logger = logger;
            _networkRepository = networkRepository;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");
            int hourSpan = 24 - DateTime.Now.Hour;
            _timer = new Timer(RemoveClientConnection, null, TimeSpan.Zero,
                TimeSpan.FromHours(hourSpan));
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
                keyValuePair.Value.Clients.ForEach(client =>
                {
                    _logger.LogInformation("Removing firewall exception for " + client);
                    firewall.RemoveException(client);
                    _logger.LogInformation("Removing from network client with ip: " + client);
                    keyValuePair.Value.RemoveClient(client);
                });
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
