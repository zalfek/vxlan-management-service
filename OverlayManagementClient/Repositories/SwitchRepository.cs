﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using OverlayManagementClient.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OverlayManagementClient.Repositories
{

    public static class SwitchRepositoryExtensions
    {
        public static void AddSwitchRepository(this IServiceCollection services)
        {
            services.AddHttpClient<ISwitchRepository, SwitchRepository>();
        }
    }
    public class SwitchRepository : ISwitchRepository
    {

        private readonly HttpClient _httpClient;
        private readonly string _Scope = string.Empty;
        private readonly string _BaseAddress = string.Empty;
        private readonly ITokenAcquisition _tokenAcquisition;

        public SwitchRepository(ITokenAcquisition tokenAcquisition, HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _tokenAcquisition = tokenAcquisition;
            _Scope = configuration["OverlayManagementService:OverlayManagementServiceScope"];
            _BaseAddress = configuration["OverlayManagementService:OverlayManagementServiceBaseAddress"];
        }

        private async Task PrepareAuthenticatedClient()
        {
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { _Scope });
            Debug.WriteLine($"access token-{accessToken}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<OpenVirtualSwitch>> GetSwitchesAsync()
        {
            await PrepareAuthenticatedClient();
            var response = await _httpClient.GetAsync($"{ _BaseAddress}/management/list/switches");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                IEnumerable<OpenVirtualSwitch> openVirtualSwitches = JsonConvert.DeserializeObject<IEnumerable<OpenVirtualSwitch>>(content);

                return openVirtualSwitches;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        public async Task<OpenVirtualSwitch> GetSwitchAsync(string key)
        {
            await PrepareAuthenticatedClient();
            var response = await _httpClient.GetAsync($"{ _BaseAddress}/management/get/switch/{key}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                OpenVirtualSwitch openVirtualSwitch = JsonConvert.DeserializeObject<OpenVirtualSwitch>(content);

                return openVirtualSwitch;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        public async void AddSwitchAsync(OvsRegistration ovsRegistration)
        {


            await PrepareAuthenticatedClient();

            byte[] data;
            using (var br = new BinaryReader(ovsRegistration.KeyFile.OpenReadStream()))
            {
                data = br.ReadBytes((int)ovsRegistration.KeyFile.OpenReadStream().Length);
            }
            ByteArrayContent bytes = new(data);
            MultipartFormDataContent multiContent = new();
            multiContent.Add(bytes, "KeyFile", ovsRegistration.KeyFile.FileName);
            multiContent.Add(new StringContent(ovsRegistration.Key.ToString()), "Key");
            multiContent.Add(new StringContent(ovsRegistration.ManagementIp), "ManagementIp");
            multiContent.Add(new StringContent(ovsRegistration.PrivateIP), "PrivateIP");
            multiContent.Add(new StringContent(ovsRegistration.PublicIP), "PublicIP");

            var response = await this._httpClient.PostAsync($"{ _BaseAddress}/management/register/switch", multiContent);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
            }
        }


        public async Task DeleteSwitchAsync(string key)
        {
            await PrepareAuthenticatedClient();

            var response = await _httpClient.DeleteAsync($"{ _BaseAddress}/management/delete/switch/{key}");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return;
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }
    }
}
