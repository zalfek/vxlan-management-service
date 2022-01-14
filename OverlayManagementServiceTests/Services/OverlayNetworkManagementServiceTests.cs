using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OverlayManagementService.Dtos;
using OverlayManagementService.Factories;
using OverlayManagementService.Network;
using OverlayManagementService.Repositories;
using System.Collections.Generic;

namespace OverlayManagementService.Services.Tests
{
    [TestClass()]
    public class OverlayNetworkManagementServiceTests
    {

        private readonly OverlayNetworkManagementService _sut;
        private readonly Mock<INetworkRepository> _networkRepositorMock = new();
        private readonly Mock<ISwitchRepository> _switchRepositoryMock = new();
        private readonly Mock<IIdentifierFactory<IPAddress>> _ipAddressFactory = new();
        private readonly Mock<ILogger<OverlayNetworkManagementService>> _loggerMock = new();
        private readonly Mock<IIdentifier> _vniMock = new();
        private readonly Mock<INetworkFactory> _networkFactoryMock = new();
        private readonly Mock<IBridgeFactory> _bridgeFactoryMock = new();
        private readonly Mock<IOpenVirtualSwitch> _openVirtualSwitchMock = new();
        private readonly Mock<IOverlayNetwork> _overlayNetworkMock = new();
        private readonly Mock<IBridge> _bridgeMock = new();
        private readonly Mock<IConfiguration> _configurationMock = new();
        private readonly Mock<ITargetDeviceManagementService> _targetDeviceManagementServiceMock = new();
        public OverlayNetworkManagementServiceTests()
        {
            _sut = new OverlayNetworkManagementService(
                _loggerMock.Object,
                _networkRepositorMock.Object,
                _vniMock.Object,
                _networkFactoryMock.Object,
                _bridgeFactoryMock.Object,
                _ipAddressFactory.Object,
                _switchRepositoryMock.Object,
                _configurationMock.Object,
                _targetDeviceManagementServiceMock.Object
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

            _vniMock.Setup(v => v.ReserveVNI()).Returns(It.IsAny<string>());
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