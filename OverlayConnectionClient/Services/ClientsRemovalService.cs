using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OverlayConnectionClient.Network;
using OverlayConnectionClient.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OverlayConnectionClient.Services
{

    /// <summary>
    /// This Service is running in background and removes all vxlan interfaces at midnight.
    /// </summary>
    public class ClientsRemovalService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<ClientsRemovalService> _logger;
        private Timer _timer;
        private readonly IInterfaceRepository _interfaceRepository;


        /// <summary>
        /// Constructor for ClientsRemovalService.
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="jsonRepository">Json file repository object</param>
        /// <returns>new ClientsRemovalService object</returns>
        public ClientsRemovalService(ILogger<ClientsRemovalService> logger, IInterfaceRepository jsonRepository)
        {
            _logger = logger;
            _interfaceRepository = jsonRepository;
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
            IDictionary<string, ILinuxVXLANInterface> networks = _interfaceRepository.GetAllInterfaces();
            foreach (KeyValuePair<string, ILinuxVXLANInterface> keyValuePair in networks)
            {
                _logger.LogInformation("Removing interface from network with id: " + keyValuePair.Value.VNI);
                _interfaceRepository.DeleteInterface(keyValuePair.Key);
                keyValuePair.Value.CleanUpInterface();
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
