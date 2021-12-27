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
using OverlayManagementClient.Models;
using System.IO;
using OverlayManagementClient.Repositories;

namespace OverlayManagementClient.Services
{
    public class VXLANManagementService : IVXLANManagementService
    {
        private readonly INetworkRepository _networkRepository;
        private readonly ISwitchRepository _switchRepository;
        private readonly IMachineRepository _machineRepository;

        public VXLANManagementService(INetworkRepository networkRepository, ISwitchRepository switchRepository, IMachineRepository machineRepository)
        {
            _networkRepository = networkRepository;
            _switchRepository = switchRepository;
            _machineRepository = machineRepository;
        }

        public void AddMachine(VmConnection vmConnection)
        {
            _machineRepository.AddMachineAsync(vmConnection);
        }

        public OverlayNetwork AddNetwork(OVSConnection oVSConnection)
        {
            return _networkRepository.AddNetworkAsync(oVSConnection).Result;
        }

        public void AddSwitch(OvsRegistration ovsRegistration)
        {
            _switchRepository.AddSwitchAsync(ovsRegistration);
        }

        public void DeleteNetwork(string groupId)
        {
            _networkRepository.DeleteNetworkAsync(groupId);
        }

        public void DeleteSwitch(string key)
        {
            _switchRepository.DeleteSwitchAsync(key);
        }

        public OverlayNetwork EditNetwork(OverlayNetwork overlayNetwork)
        {
            return _networkRepository.EditNetworkAsync(overlayNetwork).Result;
        }

        public OverlayNetwork GetNetwork(string id)
        {
            return _networkRepository.GetNetworkAsync(id).Result;
        }

        public IEnumerable<OverlayNetwork> GetNetworks()
        {
            return _networkRepository.GetNetworksAsync().Result;
        }

        public OpenVirtualSwitch GetSwitch(string key)
        {
            return _switchRepository.GetSwitchAsync(key).Result;
        }

        public IEnumerable<OpenVirtualSwitch> GetSwitches()
        {
            return _switchRepository.GetSwitchesAsync().Result;
        }

        public void RemoveMachine(string groupid, Guid guid)
        {
            _machineRepository.RemoveMachineAsync(groupid, guid);
        }
    }
}
