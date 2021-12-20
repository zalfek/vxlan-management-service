using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using OverlayConnectionClient.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OverlayConnectionClient.Repositories
{
    public class NetworkRepository : INetworkRepository
    {

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient _httpClient;
        private readonly string _Scope = string.Empty;
        private readonly string _BaseAddress = string.Empty;
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly ILogger<NetworkRepository> _logger;

        public NetworkRepository(ITokenAcquisition tokenAcquisition, HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor contextAccessor, ILogger<NetworkRepository> logger)
        {
            _httpClient = httpClient;
            _tokenAcquisition = tokenAcquisition;
            _contextAccessor = contextAccessor;
            _Scope = configuration["OverlayManagementService:OverlayManagementServiceScope"];
            _BaseAddress = configuration["OverlayManagementService:OverlayManagementServiceBaseAddress"];
            _logger = logger;
        }

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
