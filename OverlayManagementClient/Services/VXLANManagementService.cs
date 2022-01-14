using OverlayManagementClient.Models;
using OverlayManagementClient.Repositories;
using System;
using System.Collections.Generic;

namespace OverlayManagementClient.Services
{
    /// <summary>
    /// Service class(Facade) which acts as interface for all management related operations.
    /// </summary>
    public class VXLANManagementService : IVXLANManagementService
    {
        private readonly INetworkRepository _networkRepository;
        private readonly ISwitchRepository _switchRepository;
        private readonly IDeviceRepository _deviceRepository;

        public VXLANManagementService(INetworkRepository networkRepository, ISwitchRepository switchRepository, IDeviceRepository deviceRepository)
        {
            _networkRepository = networkRepository;
            _switchRepository = switchRepository;
            _deviceRepository = deviceRepository;
        }

        /// <summary>
        /// Method adds a device to the network by triggering an API call on Device Repository.
        /// </summary>
        /// <param name="vmConnection">VmConnection DTO that holds all necessary data for device deployment</param>
        public void AddTargetDevice(VmConnection vmConnection)
        {
            _deviceRepository.AddDeviceAsync(vmConnection);
        }

        /// <summary>
        /// Method invokes network deployment by triggering an API call on Network Repository.
        /// </summary>
        /// <param name="oVSConnection">OVSConnection DTO that holds all necessary data for network deployment</param>
        /// <returns>OverlayNetwork object</returns>
        public OverlayNetwork AddNetwork(OVSConnection oVSConnection)
        {
            return _networkRepository.AddNetworkAsync(oVSConnection).Result;
        }

        /// <summary>
        /// Method invokes switch regitration by triggering an API call on switch Repository.
        /// </summary>
        /// <param name="ovsRegistration">OvsRegistration DTO that holds all necessary data for switch registration</param>
        public void AddSwitch(OvsRegistration ovsRegistration)
        {
            _switchRepository.AddSwitchAsync(ovsRegistration);
        }

        /// <summary>
        /// Method invokes network clean up by triggering an API call on Network Repository.
        /// </summary>
        /// <param name="groupId">Id of a group to which the network is assigned</param>
        public void DeleteNetwork(string groupId)
        {
            _networkRepository.DeleteNetworkAsync(groupId);
        }

        /// <summary>
        /// Method invokes switch clean up by triggering an API call on Switch Repository.
        /// </summary>
        /// <param name="key">Open virtual switch prefix</param>
        public void DeleteSwitch(string key)
        {
            _switchRepository.DeleteSwitchAsync(key);
        }

        /// <summary>
        /// Method invokes network update up by triggering an API call on Network Repository.
        /// </summary>
        /// <param name="overlayNetwork">Updated OverlayNetwork object </param>
        /// <returns>OverlayNetwork object</returns>
        public OverlayNetwork EditNetwork(OverlayNetwork overlayNetwork)
        {
            return _networkRepository.EditNetworkAsync(overlayNetwork).Result;
        }

        /// <summary>
        /// Method queries the network by triggering an API call on Network Repository.
        /// </summary>
        /// <param name="vni">Virtual Network Identifier</param>
        /// <returns>OverlayNetwork object</returns>
        public OverlayNetwork GetNetwork(string vni)
        {
            return _networkRepository.GetNetworkAsync(vni).Result;
        }

        /// <summary>
        /// Method queries all networks by triggering an API call on Network Repository.
        /// </summary>
        /// <returns>IEnumerable with OverlayNetwork objects</returns>
        public IEnumerable<OverlayNetwork> GetNetworks()
        {
            return _networkRepository.GetNetworksAsync().Result;
        }

        /// <summary>
        /// Method queries switch by triggering an API call on Switch Repository.
        /// </summary>
        /// <param name="key">Open virtual switch prefix</param>
        /// <returns>OpenVirtualSwitch objects</returns>
        public OpenVirtualSwitch GetSwitch(string key)
        {
            return _switchRepository.GetSwitchAsync(key).Result;
        }

        /// <summary>
        /// Method queries all switches by triggering an API call on Switch Repository.
        /// </summary>
        /// <returns>IEnumerable with OpenVirtualSwitch objects</returns>
        public IEnumerable<OpenVirtualSwitch> GetSwitches()
        {
            return _switchRepository.GetSwitchesAsync().Result;
        }

        /// <summary>
        ///  Method removes a device from the network by triggering an API call on Device Repository.
        /// </summary>
        /// <param name="groupid">Id of a group to which the network is assigned</param>
        /// <param name="guid">Id of the target device</param>
        public void RemoveTargetDevice(string groupid, Guid guid)
        {
            _deviceRepository.RemoveDeviceAsync(groupid, guid);
        }
    }
}
