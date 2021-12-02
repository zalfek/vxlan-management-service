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
        private readonly IRepository _jsonRepository;
        private readonly IIdentifier _vni;
        private readonly IAddress IpAddress;
        private IDictionary<string, IOpenVirtualSwitch> OpenVirtualSwitches;
        private readonly INetworkFactory NetworkFactory;
        private readonly IBridgeFactory BridgeFactory;
        private readonly IVirtualMachineFactory VirtualMachineFactory;
        public VMOverlayManagementService(ILogger<VMOverlayManagementService> logger, IRepository jsonRepository, IIdentifier vni, IAddress ipAddress, INetworkFactory networkFactory,
            IBridgeFactory bridgeFactory, IVirtualMachineFactory virtualMachineFactory)
        {
            OpenVirtualSwitches = new Dictionary<string, IOpenVirtualSwitch>();
            _logger = logger;
            _jsonRepository = jsonRepository;
            _vni = vni;
            IpAddress = ipAddress;
            NetworkFactory = networkFactory;
            BridgeFactory = bridgeFactory;
            VirtualMachineFactory = virtualMachineFactory;
        }

        public void DeleteNetwork(Membership membership)
        {

            IOverlayNetwork overlayNetwork =_jsonRepository.GetOverlayNetwork(membership.MembershipId);
            overlayNetwork.CleanUpNetwork();
            _jsonRepository.DeleteOverlayNetwork(membership.MembershipId);
        }

        public IOverlayNetwork DeployNetwork(OVSConnection oVSConnection)
        {
            string vni = _vni.GenerateUniqueVNI();
            OpenVirtualSwitches[oVSConnection.Key].AddBridge(BridgeFactory.CreateBridge(Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 14), vni, OpenVirtualSwitches[oVSConnection.Key].ManagementIp));
            IOverlayNetwork overlayNetwork = NetworkFactory.CreateOverlayNetwork(vni, OpenVirtualSwitches[oVSConnection.Key]);
            _jsonRepository.SaveOverlayNetwork(oVSConnection.MembershipId, overlayNetwork);
            overlayNetwork.DeployNetwork();
            if (oVSConnection.vmConnection != null) { RegisterMachine(oVSConnection.vmConnection); }

            return overlayNetwork;
        }

        public IOverlayNetwork RegisterMachine(VmConnection vmConnection)
        {
           IOverlayNetwork overlayNetwork = _jsonRepository.GetOverlayNetwork(vmConnection.Membership);
           IVirtualMachine virtualMachine = VirtualMachineFactory.CreateVirtualMachine(Guid.NewGuid(), vmConnection.ManagementIp, overlayNetwork.VNI, IpAddress.GenerarteUniqueIPV4Address(), OpenVirtualSwitches[vmConnection.Key].PrivateIP, vmConnection.CommunicationIP);
           virtualMachine.DeployVMConnection();
           overlayNetwork.AddVMachine(virtualMachine);
           _jsonRepository.SaveOverlayNetwork(vmConnection.Membership, overlayNetwork);

            return overlayNetwork;
        }

        public IOverlayNetwork SuspendNetwork(Membership membership)
        {
            throw new NotImplementedException();
        }

        public IOverlayNetwork UnRegisterMachine(VmConnection vmConnectionInfo)
        {
            throw new NotImplementedException();
        }

        public IOpenVirtualSwitch AddSwitch(IOpenVirtualSwitch openVirtualSwitch)
        {
            OpenVirtualSwitches.Add(openVirtualSwitch.Key, openVirtualSwitch);
            return openVirtualSwitch;

        }

        public IEnumerable<IOverlayNetwork> GetAllNetworks()
        {
            return _jsonRepository.GetAllNetworks().Values.ToArray();
        }


    }
}
