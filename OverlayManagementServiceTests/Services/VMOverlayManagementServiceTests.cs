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
        private readonly Mock<INetworkRepository> _networkRepository = new();
        private readonly Mock<ISwitchRepository> _switchRepository = new();
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

        public VMOverlayManagementServiceTests()
        {
            _sut = new VMOverlayManagementService(
                _loggerMock.Object,
                _networkRepository.Object, 
                _vniMock.Object, 
                _networkFactoryMock.Object, 
                _bridgeFactoryMock.Object, 
                _virtualMachineFactoryMock.Object, 
                _ipAddressFactory.Object,
                _switchRepository.Object,
                _firewallRepositoryMock.Object,
                _firewallFactoryMock.Object
                );
        }

        [TestMethod()]
        public void DeleteNetworkTest()
        {
            string groupId = "46a2e969-c558-4892-8e45-9cc2875b8268";

            Mock<IOverlayNetwork> overlayNetworkMock = new();
            _networkRepository.Setup(x => x.GetOverlayNetwork(It.IsAny<string>())).Returns(overlayNetworkMock.Object);
            _networkRepository.Setup(x => x.DeleteOverlayNetwork(It.IsAny<string>()));

            //Act
            _sut.DeleteNetwork(groupId);

            //Assert
            _networkRepository.VerifyAll();
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
            _bridgeFactoryMock.Setup(n => n.CreateBridge(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(_bridgeMock.Object);
            _networkRepository.Setup(x => x.SaveOverlayNetwork(It.IsAny<IOverlayNetwork>())).Returns(_overlayNetworkMock.Object);
            _switchRepository.Setup(s => s.GetSwitch(It.IsAny<string>())).Returns(_openVirtualSwitchMock.Object);
            _overlayNetworkMock.Setup(x => x.DeployNetwork());

            _sut.DeployNetwork(oVSConnection);

            _networkRepository.VerifyAll();
            _overlayNetworkMock.VerifyAll();
            _switchRepository.VerifyAll();

        }

        [TestMethod()]
        public void RegisterMachineTest()
        {
            VmConnection vmConnection = new()
            {
                Key = "thu",
                ManagementIp = "255.255.255.255",
                CommunicationIP = "255.255.255.255",
                OVSIPAddress = "255.255.255.255",
                GroupId = "46a2e969-c558-4892-8e45-9cc2875b8268"

            };

            _ipAddressMock.Setup(v => v.GenerarteUniqueIPV4Address()).Returns("255.255.255.255");
            _networkRepository.Setup(x => x.GetOverlayNetwork(It.IsAny<string>())).Returns(_overlayNetworkMock.Object);
            _virtualMachineFactoryMock.Setup(v => v.CreateVirtualMachine(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(_virtualMachineMock.Object);
            _overlayNetworkMock.Setup(sw => sw.AddVMachine(It.IsAny<IVirtualMachine>()));
            _networkRepository.Setup(r => r.SaveOverlayNetwork(It.IsAny<IOverlayNetwork>()));
            _switchRepository.Setup(s => s.GetSwitch(It.IsAny<string>())).Returns(_openVirtualSwitchMock.Object);


            _sut.RegisterMachine(vmConnection);

            _networkRepository.VerifyAll();
            _virtualMachineFactoryMock.VerifyAll();
            _overlayNetworkMock.VerifyAll();
            _networkRepository.VerifyAll();
            _switchRepository.VerifyAll();
        }

        //[TestMethod()]
        //public void SuspendNetworkTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void UnRegisterMachineTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void AddSwitchTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetAllNetworksTest()
        //{
        //    Assert.Fail();
        //}
    }
}