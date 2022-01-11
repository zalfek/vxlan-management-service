using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using OverlayConnectionClient.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OverlayConnectionClient.Repositories
{
    public class NetworkRepository : INetworkRepository
    {

        private readonly HttpClient _httpClient;
        private readonly string _Scope = string.Empty;
        private readonly string _BaseAddress = string.Empty;
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly ILogger<NetworkRepository> _logger;

        public NetworkRepository(ITokenAcquisition tokenAcquisition, HttpClient httpClient, IConfiguration configuration, ILogger<NetworkRepository> logger)
        {
            _httpClient = httpClient;
            _tokenAcquisition = tokenAcquisition;
            _Scope = configuration["OverlayManagementService:OverlayManagementServiceScope"];
            _BaseAddress = configuration["OverlayManagementService:OverlayManagementServiceBaseAddress"];
            _logger = logger;
        }

        /// <summary>
        /// Method allows to get all deployed Networks from the OverlayManagementService API.
        /// </summary>
        /// <returns>IEnumerable with OverlayNetwork DTO's</returns>
        public async Task<IEnumerable<OverlayNetwork>> GetNetworksAsync()
        {
            await PrepareAuthenticatedClient();
            var request = $"{ _BaseAddress}/connection/list/networks";
            _logger.LogInformation("Sending request to API: " +request);
            var response = await _httpClient.GetAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Response from API: " + content);
                IEnumerable<OverlayNetwork> OverlayNetworkList = JsonConvert.DeserializeObject<IEnumerable<OverlayNetwork>>(content);

                return OverlayNetworkList;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        private async Task PrepareAuthenticatedClient()
        {
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { _Scope });
            _logger.LogInformation($"access token-{accessToken}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Method allows to get a Network assigned to a specific group id.
        /// This request will also trigger deployment of the vxlan interface on Open Virtual Switch towards client
        /// </summary>
        /// <param name="groupId">Id of Azure Active Derictory group for which Network was deployed</param>
        /// <returns>OverlayNetwork DTO as Task result</returns>
        public async Task<OverlayNetwork> GetNetworkAsync(string groupId)
        {
            await PrepareAuthenticatedClient();
            var request = $"{ _BaseAddress}/connection/get/network/{groupId}";
            _logger.LogInformation("Sending request to API: " + request);
            var response = await _httpClient.GetAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Response from API: " + content);
                OverlayNetwork OverlayNetwork = JsonConvert.DeserializeObject<OverlayNetwork>(content);

                return OverlayNetwork;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        /// <summary>
        /// Method allows to suspend connection to a Network with a specific group id.
        /// </summary>
        /// <param name="groupId">Id of Azure Active Derictory group for which Network was deployed</param>
        public async void RemoveClientAsync(string groupId)
        {
            await PrepareAuthenticatedClient();
            var request = $"{ _BaseAddress}/connection/suspend/connection/{groupId}";
            _logger.LogInformation("Sending request to API: " + request);
            var response = await _httpClient.GetAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Response from API: " + content);
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }


    }
}
