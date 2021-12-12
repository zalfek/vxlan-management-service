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
        private readonly Mock<IRepository> _jsonRepositoryMock = new Mock<IRepository>();
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
            _sut = new VMOverlayManagementService(_loggerMock.Object, _jsonRepositoryMock.Object, _vniMock.Object, _ipAddressMock.Object, _networkFactoryMock.Object, _bridgeFactoryMock.Object, _virtualMachineFactoryMock.Object);
        }

        [TestMethod()]
        public void DeleteNetworkTest()
        {
            string membership = "46a2e969-c558-4892-8e45-9cc2875b8268";

            Mock<IOverlayNetwork> overlayNetworkMock = new Mock<IOverlayNetwork>();
            _jsonRepositoryMock.Setup(x => x.GetOverlayNetwork(It.IsAny<string>())).Returns(overlayNetworkMock.Object);
            _jsonRepositoryMock.Setup(x => x.DeleteOverlayNetwork(It.IsAny<string>()));

            //Act
            _sut.DeleteNetwork(membership);

            //Assert
            _jsonRepositoryMock.VerifyAll();
        }

        [TestMethod()]
        public void DeployNetworkTest()
        {
            OVSConnection oVSConnection = new OVSConnection
            {
                Key = "thu",
                MembershipId = "1"
            };


            _openVirtualSwitchMock.SetupGet(x => x.Key).Returns("thu");
            _openVirtualSwitchMock.SetupGet(x => x.ManagementIp).Returns("255.255.255.255");
            _sut.AddSwitch(_openVirtualSwitchMock.Object);



            _vniMock.Setup(v => v.GenerateUniqueVNI()).Returns("1");
            _networkFactoryMock.Setup(n => n.CreateOverlayNetwork(It.IsAny<string>(), It.IsAny<IOpenVirtualSwitch>())).Returns(_overlayNetworkMock.Object);
            _bridgeFactoryMock.Setup(n => n.CreateBridge(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(_bridgeMock.Object);
            _jsonRepositoryMock.Setup(x => x.SaveOverlayNetwork(It.IsAny<string>(), It.IsAny<IOverlayNetwork>())).Returns(_overlayNetworkMock.Object);
            _overlayNetworkMock.Setup(x => x.DeployNetwork());

            _sut.DeployNetwork(oVSConnection);


            _jsonRepositoryMock.VerifyAll();
            _overlayNetworkMock.VerifyAll();


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
                Membership = "1"

            };

            _ipAddressMock.Setup(v => v.GenerarteUniqueIPV4Address()).Returns("255.255.255.255");
            _overlayNetworkMock.SetupGet(n => n.VNI).Returns("1");
            _openVirtualSwitchMock.SetupGet(sw => sw.PrivateIP).Returns("255.255.255.255");
            _jsonRepositoryMock.Setup(x => x.GetOverlayNetwork(It.IsAny<string>())).Returns(_overlayNetworkMock.Object);
            _virtualMachineFactoryMock.Setup(v => v.CreateVirtualMachine(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(_virtualMachineMock.Object);
            _virtualMachineMock.Setup(v => v.DeployVMConnection());
            _overlayNetworkMock.Setup(sw => sw.AddVMachine(_virtualMachineMock.Object));
            _jsonRepositoryMock.Setup(r => r.SaveOverlayNetwork(It.IsAny<string>(), It.IsAny<IOverlayNetwork>()));
            _openVirtualSwitchMock.SetupGet(x => x.Key).Returns("thu");

            _sut.AddSwitch(_openVirtualSwitchMock.Object);
            _sut.RegisterMachine(vmConnection);

            _jsonRepositoryMock.VerifyAll();
            _virtualMachineFactoryMock.VerifyAll();
            _virtualMachineMock.VerifyAll();
            _overlayNetworkMock.VerifyAll();
            _jsonRepositoryMock.VerifyAll();
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