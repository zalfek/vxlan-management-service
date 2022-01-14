using Newtonsoft.Json;
using OverlayManagementService.Dtos;
using OverlayManagementService.Models;
using OverlayManagementService.Network;
using System;
using System.Collections.Generic;

namespace OverlayManagementService.Network
{
    /// <summary>
    /// Class encapsulates functionality required for neteork deployment, cleanup, connecting target devices and clients.
    /// </summary>
    public class VXLANOverlayNetwork : IOverlayNetwork
    {
        public VXLANOverlayNetwork()
        {
        }

        public VXLANOverlayNetwork(string groupId, string vNI, IOpenVirtualSwitch openVirtualSwitch, IAddress ipAddress)
        {
            Vni = vNI;
            GroupId = groupId;
            OpenVirtualSwitch = openVirtualSwitch;
            TargetDevices = new List<ITargetDevice>();
            Clients = new List<Student>();
            IpAddress = ipAddress;
        }

        public string Vni { get; set; }
        public string GroupId { get; set; }
        public IOpenVirtualSwitch OpenVirtualSwitch { get; set; }
        public List<ITargetDevice> TargetDevices { get; set; }
        public List<Student> Clients { get; set; }
        public IAddress IpAddress { get; set; }

        /// <summary>
        /// Method triggers deployment of the VXLAN connection towards client.
        /// </summary>
        /// <param name="client">Student DTO object</param>
        public string AddClient(Student client)
        {
            Clients.Add(client);
            OpenVirtualSwitch.DeployClientVXLANInterface(Vni, client.IpAddress);
            return IpAddress.GenerarteUniqueIPV4Address();
        }

        /// <summary>
        /// Method triggers deployment of the VXLAN connection towards taregt device.
        /// </summary>
        /// <param name="taregtDevice">TargetDevice object</param>
        public void AddTargetDevice(ITargetDevice taregtDevice)
        {
            taregtDevice.DeployVMConnection(IpAddress.GenerarteUniqueIPV4Address());
            OpenVirtualSwitch.DeployVXLANInterface(taregtDevice);
            TargetDevices.Add(taregtDevice);
        }

        /// <summary>
        /// Method triggers cleanup of entire network. All devices are removed from the network and network is being deleted.
        /// </summary>
        public void CleanUpNetwork()
        {
            TargetDevices.ForEach(vm => { 
                vm.CleanUpVMConnection();
                OpenVirtualSwitch.RemoveTargetConnection(vm);
            });
            OpenVirtualSwitch.CleanUpBridge(Vni);
        }

        /// <summary>
        /// Method triggers network deployment.
        /// </summary>
        public void DeployNetwork()
        {
            OpenVirtualSwitch.DeployBridge(Vni);
        }

        /// <summary>
        /// Method triggers client removal from network.
        /// </summary>
        /// <param name="client">Student DTO object</param>
        public void RemoveClient(Student client)
        {
            for (int i = Clients.Count - 1; i > -1; --i)
            {
                if (Clients[i].IpAddress == client.IpAddress) { 
                    Clients.RemoveAt(i);
                }
            }
            OpenVirtualSwitch.CleanUpClientVXLANInterface(Vni, client.IpAddress);
        }

        /// <summary>
        /// Method triggers target device removal from the network.
        /// </summary>
        /// <param name="guid">Guid of the target device</param>
        public void RemoveTargetDevice(Guid guid)
        {
            ITargetDevice virtualMachine = TargetDevices.Find(x => x.Guid == guid);
            OpenVirtualSwitch.RemoveTargetConnection(virtualMachine);
            virtualMachine.CleanUpVMConnection();
            TargetDevices.Remove(virtualMachine);
        }

    }
}
