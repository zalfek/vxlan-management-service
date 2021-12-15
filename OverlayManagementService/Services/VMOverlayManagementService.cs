using Microsoft.Extensions.Logging;
using OverlayManagementService.Dtos;
using OverlayManagementService.Factories;
using OverlayManagementService.Network;
using OverlayManagementService.Repositories;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Services
{
    public class VMOverlayManagementService : IOverlayManagementService
    {

        private readonly ILogger<VMOverlayManagementService> _logger;
        private readonly INetworkRepository _networkRepository;
        private readonly ISwitchRepository _switchRepository;
        private readonly IIdentifier _vniResolver;
        private readonly IIdentifierFactory<IPAddress> _ipResolverFactory;
        private readonly INetworkFactory NetworkFactory;
        private readonly IBridgeFactory BridgeFactory;
        private readonly IVirtualMachineFactory VirtualMachineFactory;
        public VMOverlayManagementService(
            ILogger<VMOverlayManagementService> logger,
            INetworkRepository networkRepository,
            IIdentifier vniResolver,
            INetworkFactory networkFactory,
            IBridgeFactory bridgeFactory,
            IVirtualMachineFactory virtualMachineFactory,
            IIdentifierFactory<IPAddress> ipResolverFactory,
            ISwitchRepository switchRepository
            )
        {
            _logger = logger;
            _networkRepository = networkRepository;
            _vniResolver = vniResolver;
            _ipResolverFactory = ipResolverFactory;
            NetworkFactory = networkFactory;
            BridgeFactory = bridgeFactory;
            VirtualMachineFactory = virtualMachineFactory;
            _switchRepository = switchRepository;
        }

        public void DeleteNetwork(string groupId)
        {
            IOverlayNetwork overlayNetwork = _networkRepository.GetOverlayNetwork(groupId);
            overlayNetwork.CleanUpNetwork();
            _networkRepository.DeleteOverlayNetwork(groupId);
        }

        public IOverlayNetwork DeployNetwork(OVSConnection oVSConnection)
        {
            IOpenVirtualSwitch openVirtualSwitch = _switchRepository.GetSwitch(oVSConnection.Key);
            string vni = _vniResolver.GenerateUniqueVNI();
            openVirtualSwitch.AddBridge(
                BridgeFactory.CreateBridge(
                    Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 14),
                    vni,
                    openVirtualSwitch.ManagementIp)
                );
            IOverlayNetwork overlayNetwork = NetworkFactory.CreateOverlayNetwork(
                oVSConnection.GroupId,
                vni,
                openVirtualSwitch,
                _ipResolverFactory.CreateAddressResolver()
                );
            _networkRepository.SaveOverlayNetwork(overlayNetwork);
            overlayNetwork.DeployNetwork();
            if (oVSConnection.vmConnection != null) { RegisterMachine(oVSConnection.vmConnection); }

            return overlayNetwork;
        }

        public IOverlayNetwork RegisterMachine(VmConnection vmConnection)
        {
            IOverlayNetwork overlayNetwork = _networkRepository.GetOverlayNetwork(vmConnection.GroupId);
            IOpenVirtualSwitch openVirtualSwitch = _switchRepository.GetSwitch(vmConnection.Key);
            IVirtualMachine virtualMachine = VirtualMachineFactory.CreateVirtualMachine(
                Guid.NewGuid(),
                vmConnection.ManagementIp,
                overlayNetwork.Vni,
                openVirtualSwitch.PrivateIP, 
                vmConnection.CommunicationIP
                );
            overlayNetwork.AddVMachine(virtualMachine);
            _networkRepository.SaveOverlayNetwork(overlayNetwork);
            return overlayNetwork;
        }

        public IOverlayNetwork SuspendNetwork(string membership)
        {
            throw new NotImplementedException();
        }

        public IOverlayNetwork UnRegisterMachine(VmConnection vmConnectionInfo)
        {
            throw new NotImplementedException();
        }

        public IOpenVirtualSwitch AddSwitch(IOpenVirtualSwitch openVirtualSwitch)
        {
            _switchRepository.SaveSwitch(openVirtualSwitch);
            return openVirtualSwitch;

        }

        public IEnumerable<IOverlayNetwork> GetAllNetworks()
        {
            return _networkRepository.GetAllNetworks().Values.ToArray();
        }

        public IOverlayNetwork GetNetworkByVni(string vni)
        {
            return _networkRepository.GetOverlayNetworkByVni(vni);
        }

        public IEnumerable<IOpenVirtualSwitch> GetAllSwitches()
        {
            return _switchRepository.GetAllSwitches().Values.ToArray();
        }

        public IOpenVirtualSwitch GetSwitch(string key)
        {
            return _switchRepository.GetSwitch(key);
        }

        public IOverlayNetwork UpdateNetwork(IOverlayNetwork overlayNetwork)
        {
            return _networkRepository.SaveOverlayNetwork(overlayNetwork);
        }
    }
}
