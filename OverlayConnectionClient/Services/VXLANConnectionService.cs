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

namespace OverlayManagementClient.Services
{


    public static class VXLANConnectionServiceExtensions
    {
        public static void AddVXLANManagementService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IVXLANConnectionService, VXLANConnectionService>();
        }
    }


    public class VXLANConnectionService :IVXLANConnectionService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient _httpClient;
        private readonly string _Scope = string.Empty;
        private readonly string _BaseAddress = string.Empty;
        private readonly ITokenAcquisition _tokenAcquisition;

        public VXLANConnectionService(ITokenAcquisition tokenAcquisition, HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _httpClient = httpClient;
            _tokenAcquisition = tokenAcquisition;
            _contextAccessor = contextAccessor;
            _Scope = configuration["OverlayManagementService:OverlayManagementServiceScope"];
            _BaseAddress = configuration["OverlayManagementService:OverlayManagementServiceBaseAddress"];
        }

        public async Task<IEnumerable<OverlayNetwork>> GetNetworksAsync()
        {
            await PrepareAuthenticatedClient();
            var response = await _httpClient.GetAsync($"{ _BaseAddress}/connection/list/networks");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                IEnumerable<OverlayNetwork> OverlayNetworkList = JsonConvert.DeserializeObject<IEnumerable<OverlayNetwork>>(content);

                return OverlayNetworkList;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        private async Task PrepareAuthenticatedClient()
        {
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { _Scope });
            Debug.WriteLine($"access token-{accessToken}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<OverlayNetwork> GetNetworkAsync(string id)
        {
            await PrepareAuthenticatedClient();
            var response = await _httpClient.GetAsync($"{ _BaseAddress}/connection/get/network/{id}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                OverlayNetwork OverlayNetwork = JsonConvert.DeserializeObject<OverlayNetwork>(content);

                return OverlayNetwork;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }
    }
}
