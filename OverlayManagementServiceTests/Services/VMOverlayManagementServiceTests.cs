using OverlayManagementService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OverlayManagementService.Dtos;
using OverlayManagementService.Factories;
using OverlayManagementService.Network;
using OverlayManagementService.Repositories;
using System;
using System.Collections.Generic;

namespace OverlayManagementService.Services.Tests
{
    [TestClass()]
    public class VMOverlayManagementServiceTests
    {

        private readonly VMOverlayManagementService _sut;
        private readonly Mock<INetworkRepository> _networkRepositorMock = new();
        private readonly Mock<ISwitchRepository> _switchRepositoryMock = new();
        private readonly Mock<IIdentifierFactory<IPAddress>> _ipAddressFactory = new();
        private readonly Mock<ILogger<VMOverlayManagementService>> _loggerMock = new();
        private readonly Mock<IIdentifier> _vniMock = new();
        private readonly Mock<IAddress> _ipAddressMock = new();
        private readonly Mock<INetworkFactory> _networkFactoryMock = new();
        private readonly Mock<IBridgeFactory> _bridgeFactoryMock = new();
        private readonly Mock<IOpenVirtualSwitch> _openVirtualSwitchMock = new();
        private readonly Mock<IOverlayNetwork> _overlayNetworkMock = new();
        private readonly Mock<IBridge> _bridgeMock = new();
        private readonly Mock<IVirtualMachineFactory> _virtualMachineFactoryMock = new();
        private readonly Mock<IVirtualMachine> _virtualMachineMock = new();
        private readonly Mock<IFirewallFactory> _firewallFactoryMock = new();
        private readonly Mock<IFirewallRepository> _firewallRepositoryMock = new();
        private readonly Mock<IKeyKeeper> _KeyKeeperMock = new();
        private readonly Mock<IOpenVirtualSwitchFactory> _openVirtualSwitchFactoryMock = new();
        private readonly Mock<IConfiguration> _configurationMock = new();
        private readonly Mock<IFirewall> _firewallMock = new();

        public VMOverlayManagementServiceTests()
        {
            _sut = new VMOverlayManagementService(
                _loggerMock.Object,
                _networkRepositorMock.Object,
                _vniMock.Object,
                _networkFactoryMock.Object,
                _bridgeFactoryMock.Object,
                _virtualMachineFactoryMock.Object,
                _ipAddressFactory.Object,
                _switchRepositoryMock.Object,
                _firewallRepositoryMock.Object,
                _firewallFactoryMock.Object,
                _openVirtualSwitchFactoryMock.Object,
                _KeyKeeperMock.Object,
                _configurationMock.Object
                );
        }

        [TestMethod()]
        public void DeleteNetworkTest()
        {
            string groupId = "46a2e969-c558-4892-8e45-9cc2875b8268";

            Mock<IOverlayNetwork> overlayNetworkMock = new();
            _networkRepositorMock.Setup(x => x.GetOverlayNetwork(It.IsAny<string>())).Returns(overlayNetworkMock.Object);
            _networkRepositorMock.Setup(x => x.DeleteOverlayNetwork(It.IsAny<string>()));

            //Act
            _sut.DeleteNetwork(groupId);

            //Assert
            _networkRepositorMock.VerifyAll();
        }

        [TestMethod()]
        public void DeployNetworkTest()
        {
            OVSConnection oVSConnection = new()
            {
                Key = "thu",
                GroupId = "1"
            };

            _vniMock.Setup(v => v.GenerateUniqueVNI()).Returns(It.IsAny<string>());
            _networkFactoryMock.Setup(n => n.CreateOverlayNetwork(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IOpenVirtualSwitch>(), It.IsAny<IAddress>())).Returns(_overlayNetworkMock.Object);
            _bridgeFactoryMock.Setup(n => n.CreateBridge(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(_bridgeMock.Object);
            _networkRepositorMock.Setup(x => x.SaveOverlayNetwork(It.IsAny<IOverlayNetwork>())).Returns(_overlayNetworkMock.Object);
            _switchRepositoryMock.Setup(s => s.GetSwitch(It.IsAny<string>())).Returns(_openVirtualSwitchMock.Object);
            _overlayNetworkMock.Setup(x => x.DeployNetwork());

            _sut.DeployNetwork(oVSConnection);

            _networkRepositorMock.VerifyAll();
            _overlayNetworkMock.VerifyAll();
            _switchRepositoryMock.VerifyAll();

        }

        [TestMethod()]
        public void RegisterMachineTest()
        {
            VmConnection vmConnection = new()
            {
                ManagementIp = "255.255.255.255",
                CommunicationIP = "255.255.255.255",
                GroupId = "46a2e969-c558-4892-8e45-9cc2875b8268"

            };

            _ipAddressMock.Setup(v => v.GenerarteUniqueIPV4Address()).Returns(It.IsAny<string>());
            _networkRepositorMock.Setup(x => x.GetOverlayNetwork(It.IsAny<string>())).Returns(_overlayNetworkMock.Object);
            _virtualMachineFactoryMock.Setup(v => v.CreateVirtualMachine(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(_virtualMachineMock.Object);
            _overlayNetworkMock.Setup(sw => sw.AddVMachine(It.IsAny<IVirtualMachine>()));
            _overlayNetworkMock.SetupGet(x => x.Vni).Returns(It.IsAny<string>());
            _overlayNetworkMock.SetupGet(x => x.OpenVirtualSwitch).Returns(_openVirtualSwitchMock.Object);
            _openVirtualSwitchMock.SetupGet(x => x.PrivateIP).Returns(It.IsAny<string>());
            _networkRepositorMock.Setup(r => r.SaveOverlayNetwork(It.IsAny<IOverlayNetwork>()));



            _sut.RegisterMachine(vmConnection);

            _networkRepositorMock.VerifyAll();
            _virtualMachineFactoryMock.VerifyAll();
            _overlayNetworkMock.VerifyAll();
            _networkRepositorMock.VerifyAll();

        }

        [TestMethod()]
        public void UnRegisterMachineTest()
        {
            string groupId = "46a2e969-c558-4892-8e45-9cc2875b8268";
            Guid guid = Guid.NewGuid();
            _networkRepositorMock.Setup(x => x.GetOverlayNetwork(It.IsAny<string>())).Returns(_overlayNetworkMock.Object);
            _overlayNetworkMock.Setup(n => n.RemoveVMachine(It.IsAny<Guid>()));
            _networkRepositorMock.Setup(r => r.SaveOverlayNetwork(It.IsAny<IOverlayNetwork>()));


            _sut.UnRegisterMachine(groupId, guid);


            _networkRepositorMock.VerifyAll();
            _overlayNetworkMock.VerifyAll();
        }

        [TestMethod()]
        public void AddSwitchTest()
        {

            OvsRegistration ovsRegistration = new()
            {
                ManagementIp = "255.255.255.255",
                PrivateIP = "255.255.255.255",
                PublicIP = "255.255.255.255",
                Key = "thu",

            };


            _openVirtualSwitchFactoryMock.Setup(sf => sf.CreateSwitch(ovsRegistration)).Returns(_openVirtualSwitchMock.Object);
            _openVirtualSwitchMock.SetupGet(x => x.ManagementIp).Returns(It.IsAny<string>());
            _switchRepositoryMock.Setup(sw => sw.SaveSwitch(_openVirtualSwitchMock.Object));
            _firewallFactoryMock.Setup(ff => ff.CreateFirewall(It.IsAny<string>())).Returns(_firewallMock.Object);
            _firewallRepositoryMock.Setup(fr => fr.AddFirewall(It.IsAny<string>(), _firewallMock.Object));

            _sut.AddSwitch(ovsRegistration);

            _openVirtualSwitchFactoryMock.VerifyAll();
            _switchRepositoryMock.VerifyAll();
            _firewallFactoryMock.VerifyAll();
            _firewallRepositoryMock.VerifyAll();
        }

        [TestMethod()]
        public void RemoveSwitchTest()
        {
            var networks = new Dictionary<string, IOverlayNetwork>()
            {
                {"46a2e969-c558-4892-8e45-9cc2875b8268", _overlayNetworkMock.Object }
            };

            _networkRepositorMock.Setup(nr => nr.GetAllNetworks()).Returns(networks);
            _overlayNetworkMock.SetupGet(x => x.OpenVirtualSwitch).Returns(_openVirtualSwitchMock.Object);
            _openVirtualSwitchMock.SetupGet(x => x.Key).Returns(It.IsAny<string>());
            _switchRepositoryMock.Setup(sw => sw.DeleteSwitch(It.IsAny<string>()));
            _firewallRepositoryMock.Setup(fr => fr.RemoveFirewall(It.IsAny<string>()));

            _sut.RemoveSwitch("thu");

            _switchRepositoryMock.VerifyAll();
            _firewallRepositoryMock.VerifyAll();
            _networkRepositorMock.VerifyAll();
        }

        [TestMethod()]
        public void GetAllNetworksTest()
        {
            var networks = new Dictionary<string, IOverlayNetwork>()
            {
                {"46a2e969-c558-4892-8e45-9cc2875b8268", _overlayNetworkMock.Object }
            };
            _networkRepositorMock.Setup(nr => nr.GetAllNetworks()).Returns(networks);

            _sut.GetAllNetworks();

            _networkRepositorMock.VerifyAll();

        }

        [TestMethod()]
        public void GetNetworkByVniTest()
        {
            _networkRepositorMock.Setup(nr => nr.GetOverlayNetworkByVni(It.IsAny<string>())).Returns(_overlayNetworkMock.Object);

            _sut.GetNetworkByVni("1");

            _networkRepositorMock.VerifyAll();
        }

        [TestMethod()]
        public void GetAllSwitchesTest()
        {
            var switches = new Dictionary<string, IOpenVirtualSwitch>()
            {
                {"46a2e969-c558-4892-8e45-9cc2875b8268", _openVirtualSwitchMock.Object }
            };

            _switchRepositoryMock.Setup(sr => sr.GetAllSwitches()).Returns(switches);

            _sut.GetAllSwitches();

            _switchRepositoryMock.Verify();
        }

        [TestMethod()]
        public void GetSwitchTest()
        {
            _switchRepositoryMock.Setup(sr => sr.GetSwitch(It.IsAny<string>())).Returns(_openVirtualSwitchMock.Object);

            _sut.GetSwitch("thu");

            _switchRepositoryMock.Verify();
        }

        [TestMethod()]
        public void UpdateNetworkTest()
        {
            string groupId = "46a2e969-c558-4892-8e45-9cc2875b8268";
            _networkRepositorMock.Setup(nr => nr.GetOverlayNetwork(groupId)).Returns(_overlayNetworkMock.Object);
            _overlayNetworkMock.SetupGet(n => n.GroupId).Returns(groupId);
            _overlayNetworkMock.Setup(n => n.CleanUpNetwork());
            _overlayNetworkMock.Setup(n => n.DeployNetwork());


            _sut.UpdateNetwork(_overlayNetworkMock.Object);

            _networkRepositorMock.VerifyAll();
            _overlayNetworkMock.VerifyAll();

        }
    }
}