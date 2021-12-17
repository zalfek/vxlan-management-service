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
        private readonly Mock<INetworkRepository> _networkRepository = new Mock<INetworkRepository>();
        private readonly Mock<ISwitchRepository> _switchRepository = new Mock<ISwitchRepository>();
        private readonly Mock<IIdentifierFactory<IPAddress>> _ipAddressFactory = new Mock<IIdentifierFactory<IPAddress>>();
        private readonly Mock<ILogger<VMOverlayManagementService>> _loggerMock = new Mock<ILogger<VMOverlayManagementService>>();
        private readonly Mock<IIdentifier> _vniMock = new Mock<IIdentifier>();
        private readonly Mock<IAddress> _ipAddressMock = new Mock<IAddress>();
        private readonly Mock<INetworkFactory> _networkFactoryMock = new Mock<INetworkFactory>();
        private readonly Mock<IBridgeFactory> _bridgeFactoryMock = new Mock<IBridgeFactory>();
        private readonly Mock<IOpenVirtualSwitch> _openVirtualSwitchMock = new Mock<IOpenVirtualSwitch>();
        private readonly Mock<IOverlayNetwork> _overlayNetworkMock = new Mock<IOverlayNetwork>();
        private readonly Mock<IBridge> _bridgeMock = new Mock<IBridge>();
        private readonly Mock<IVirtualMachineFactory> _virtualMachineFactoryMock = new Mock<IVirtualMachineFactory>();
        private readonly Mock<IVirtualMachine> _virtualMachineMock = new Mock<IVirtualMachine>();

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
                _switchRepository.Object
                );
        }

        [TestMethod()]
        public void DeleteNetworkTest()
        {
            string groupId = "46a2e969-c558-4892-8e45-9cc2875b8268";

            Mock<IOverlayNetwork> overlayNetworkMock = new Mock<IOverlayNetwork>();
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
            OVSConnection oVSConnection = new OVSConnection
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
            VmConnection vmConnection = new VmConnection
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