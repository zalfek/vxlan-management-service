using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using OverlayManagementClient.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OverlayManagementClient.Repositories
{
    public static class DeviceRepositoryExtensions
    {
        public static void AddMachineRepository(this IServiceCollection services)
        {
            services.AddHttpClient<IDeviceRepository, DeviceRepository>();
        }
    }

    /// <summary>
    /// Class encapsulates logic for target device related communication with API.
    /// </summary>
    public class DeviceRepository : IDeviceRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _Scope = string.Empty;
        private readonly string _BaseAddress = string.Empty;
        private readonly ITokenAcquisition _tokenAcquisition;

        public DeviceRepository(ITokenAcquisition tokenAcquisition, HttpClient httpClient, IConfiguration configuration)
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

        /// <summary>
        /// Method allows to deploy a device to the network by calling OverlayManagementService API.
        /// </summary>
        /// <param name="vmConnection">VmConnection DTO containing all neceserry data from device deployment</param>
        public async void AddDeviceAsync(VmConnection vmConnection)
        {
            await PrepareAuthenticatedClient();

            byte[] data;
            using (var br = new BinaryReader(vmConnection.KeyFile.OpenReadStream()))
            {
                data = br.ReadBytes((int)vmConnection.KeyFile.OpenReadStream().Length);
            }
            ByteArrayContent bytes = new(data);
            MultipartFormDataContent multiContent = new()
            {
                { bytes, "KeyFile", vmConnection.KeyFile.FileName },
                { new StringContent(vmConnection.ManagementIp), "ManagementIp" },
                { new StringContent(vmConnection.GroupId), "GroupId" },
                { new StringContent(vmConnection.CommunicationIP), "CommunicationIP" }
            };

            var response = await this._httpClient.PostAsync($"{ _BaseAddress}/device/deploy", multiContent);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
            }
        }
        /// <summary>
        /// Method allows to remove target device from network by calling OverlayManagementService API.
        /// </summary>
        /// <param name="groupId">Id of Azure Active Derictory group for which Network was deployed</param>
        /// <param name="guid">guid of the device to be removed from network</param>
        public async void RemoveDeviceAsync(string groupId, Guid guid)
        {
            await PrepareAuthenticatedClient();

            var response = await this._httpClient.GetAsync($"{ _BaseAddress}/device/suspend?groupid={groupId}&guid={guid}");

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
            }
        }

    }
}
