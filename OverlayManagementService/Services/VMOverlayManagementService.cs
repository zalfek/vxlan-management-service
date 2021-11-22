using Microsoft.Extensions.Logging;
using OverlayManagementService.DataTransferObjects;
using OverlayManagementService.Dtos;
using OverlayManagementService.Network;
using OverlayManagementService.Repositories;
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
        private readonly IOpenVirtualSwitch _openVirtualSwitch;
        private readonly IAddress IpAddress;
        public VMOverlayManagementService(ILogger<VMOverlayManagementService> logger, IRepository jsonRepository, IIdentifier vni, IAddress ipAddress)
        {
            _logger = logger;
            _jsonRepository = jsonRepository;
            _vni = vni;
            _openVirtualSwitch = new OpenVirtualSwitch();
            IpAddress = ipAddress;
        }

        public void DeleteNetwork(IMembership membership)
        {

            IOverlayNetwork overlayNetwork =_jsonRepository.GetOverlayNetwork(membership.MembershipId);
            overlayNetwork.CleanUpNetwork();
            _jsonRepository.DeleteOverlayNetwork(membership.MembershipId);
        }

        public IOverlayNetwork DeployNetwork(IVmConnectionInfo vmConnectionInfo)
        {

            IVeth veth = new Veth(Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 14), IpAddress.GenerarteUniqueIPV4Address());
            IBridge bridge = new Bridge(Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0,14), veth);
            IVXLANInterface vXLANInterface = new VXLANInterface(Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 14),  "vxlan", vmConnectionInfo.IPAddress, "10", "10", "4789");
            _openVirtualSwitch.AddBridge(bridge);

            List<IOpenVirtualSwitch> openVirtualSwitches = new List<IOpenVirtualSwitch>();
            openVirtualSwitches.Add(_openVirtualSwitch);

            IVirtualMachine virtualMachine = new VirtualMachine(Guid.NewGuid(), vmConnectionInfo);
            List<IVirtualMachine> virtualMachines = new List<IVirtualMachine>();
            virtualMachines.Add(virtualMachine);


            IOverlayNetwork overlayNetwork = new VXLANOverlayNetwork(_vni.GenerateUniqueVNI(), openVirtualSwitches, virtualMachines, new List<IUser>());


            _jsonRepository.SaveOverlayNetwork(vmConnectionInfo.Membership, overlayNetwork);


            overlayNetwork.DeployNetwork();

            return overlayNetwork;
        }

        public IOverlayNetwork RegisterMachine(IVmConnectionInfo vmConnectionInfo)
        {
           IOverlayNetwork overlayNetwork = _jsonRepository.GetOverlayNetwork(vmConnectionInfo.Membership);
            IVirtualMachine virtualMachine = new VirtualMachine(Guid.NewGuid(), vmConnectionInfo);
            virtualMachine.DeployVMConnection(IpAddress.GenerarteUniqueIPV4Address(), overlayNetwork);
            return overlayNetwork;
        }

        public IOverlayNetwork SuspendNetwork(IMembership membership)
        {
            throw new NotImplementedException();
        }

        public IOverlayNetwork UnRegisterMachine(IVmConnectionInfo vmConnectionInfo)
        {
            throw new NotImplementedException();
        }
    }
}
