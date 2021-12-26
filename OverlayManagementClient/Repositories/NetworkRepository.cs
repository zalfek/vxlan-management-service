﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using OverlayManagementClient.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OverlayManagementClient.Repositories
{

    public static class NetworkRepositoryExtensions
    {
        public static void AddNetworkRepository(this IServiceCollection services)
        {
            services.AddHttpClient<INetworkRepository, NetworkRepository>();
        }
    }

    public class NetworkRepository : INetworkRepository
    {

        private readonly HttpClient _httpClient;
        private readonly string _Scope = string.Empty;
        private readonly string _BaseAddress = string.Empty;
        private readonly ITokenAcquisition _tokenAcquisition;

        public NetworkRepository(ITokenAcquisition tokenAcquisition, HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _tokenAcquisition = tokenAcquisition;
            _Scope = configuration["OverlayManagementService:OverlayManagementServiceScope"];
            _BaseAddress = configuration["OverlayManagementService:OverlayManagementServiceBaseAddress"];
        }

        public async Task<OverlayNetwork> AddNetworkAsync(OVSConnection oVSConnection)
        {
            await PrepareAuthenticatedClient();

            var jsonRequest = JsonConvert.SerializeObject(oVSConnection);
            var jsoncontent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await this._httpClient.PostAsync($"{ _BaseAddress}/management/deploy/network", jsoncontent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                OverlayNetwork overlayNetwork = JsonConvert.DeserializeObject<OverlayNetwork>(content);

                return overlayNetwork;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        public async Task DeleteNetworkAsync(string groupId)
        {
            await PrepareAuthenticatedClient();

            var response = await _httpClient.DeleteAsync($"{ _BaseAddress}/management/delete/network/{groupId}");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        public async Task<OverlayNetwork> EditNetworkAsync(OverlayNetwork overlayNetwork)
        {
            await PrepareAuthenticatedClient();

            var jsonRequest = JsonConvert.SerializeObject(overlayNetwork);
            var jsoncontent = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");
            var response = await _httpClient.PatchAsync($"{ _BaseAddress}/management/OverlayNetworklist/{overlayNetwork.GroupId}", jsoncontent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                overlayNetwork = JsonConvert.DeserializeObject<OverlayNetwork>(content);

                return overlayNetwork;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        public async Task<IEnumerable<OverlayNetwork>> GetNetworksAsync()
        {
            await PrepareAuthenticatedClient();
            var response = await _httpClient.GetAsync($"{ _BaseAddress}/management/list/networks");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                IEnumerable<OverlayNetwork> overlayNetworklist = JsonConvert.DeserializeObject<IEnumerable<OverlayNetwork>>(content);

                return overlayNetworklist;
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

        public async Task<OverlayNetwork> GetNetworkAsync(string vni)
        {
            await PrepareAuthenticatedClient();
            var response = await _httpClient.GetAsync($"{ _BaseAddress}/management/get/network/{vni}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                OverlayNetwork overlayNetwork = JsonConvert.DeserializeObject<OverlayNetwork>(content);

                return overlayNetwork;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }
    }
}