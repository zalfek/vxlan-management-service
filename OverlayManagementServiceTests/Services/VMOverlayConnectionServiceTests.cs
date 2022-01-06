using Microsoft.VisualStudio.TestTools.UnitTesting;
using OverlayManagementService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using OverlayManagementService.Dtos;
using OverlayManagementService.Repositories;
using OverlayManagementService.Network;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using OverlayManagementService.Factories;
using OverlayManagementService.Models;

namespace OverlayManagementService.Services.Tests
{
    [TestClass()]
    public class VMOverlayConnectionServiceTests
    {
        private readonly OverlayNetworkConnectionService _sut;
        private readonly Mock<INetworkRepository> _networkRepositoryMock = new();
        private readonly Mock<ILogger<OverlayNetworkConnectionService>> _loggerMock = new();
        private readonly Mock<IClientConnectionFactory> _clientConnectionFactoryMock = new();
        private readonly Mock<IFirewallFactory> _firewallFactoryMock = new();
        private readonly Mock<IFirewallRepository> _firewallRepositoryMock = new();
        private readonly Mock<IOverlayNetwork> _overlayNetworkMock = new();
        private readonly Mock<IOpenVirtualSwitch> _openVirtualSwitchMock = new();
        private readonly Mock<IFirewall> _firewallMock = new();
        public VMOverlayConnectionServiceTests()
        {
            _sut = new OverlayNetworkConnectionService(_networkRepositoryMock.Object, _loggerMock.Object, _clientConnectionFactoryMock.Object, _firewallRepositoryMock.Object);
        }

        [TestMethod()]
        public void GetAllNetworksTest()
        {
            List<Claim> claims = new()
            {
                new Claim("group", "adggjdasd45t54zuw46us"),
                new Claim("group", "adgasdajkjdjghzuw46us"),
                new Claim("group", "adgsdfgaz5645f90mmsds")
            };

            ClientConnection clientConnection = new("1", "adggjdasd45t54zuw46us", "255.255.255.255", "255.255.255.255");
            Mock<IOverlayNetwork> overlayNetworkMock = new();
            _networkRepositoryMock.Setup(x => x.GetOverlayNetwork(It.IsAny<Claim>())).Returns(overlayNetworkMock.Object);
            _clientConnectionFactoryMock.Setup(c => c.CreateClientConnectionDto(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(clientConnection);
            overlayNetworkMock.SetupGet(x => x.Vni).Returns("1");
            overlayNetworkMock.SetupGet(x => x.OpenVirtualSwitch.PublicIP).Returns("255.255.255.255");

            _sut.GetAllNetworks(claims);

            _networkRepositoryMock.VerifyAll();
            _clientConnectionFactoryMock.VerifyAll();
        }

        [TestMethod()]
        public void CreateConnectionTest()
        {
            Student student = new()
            {
                Name = "Name",
                IpAddress = "255.255.255.255"
            };

            Mock<IOverlayNetwork> overlayNetworkMock = new();
            Mock<IFirewall> _firewallMock = new();
            _networkRepositoryMock.Setup(x => x.GetOverlayNetwork(It.IsAny<string>())).Returns(overlayNetworkMock.Object);
            overlayNetworkMock.Setup(n => n.AddClient(It.IsAny<Student>()));
            ClientConnection clientConnection = new("1", "adggjdasd45t54zuw46us", "255.255.255.255", "255.255.255.255");
            _clientConnectionFactoryMock.Setup(c => c.CreateClientConnectionDto(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(clientConnection);
            overlayNetworkMock.SetupGet(x => x.Vni).Returns("1");
            overlayNetworkMock.SetupGet(x => x.OpenVirtualSwitch.PublicIP).Returns("255.255.255.255");
            _firewallRepositoryMock.Setup(f => f.GetFirewall(It.IsAny<string>())).Returns(_firewallMock.Object);
            _firewallMock.Setup(f => f.AddException(It.IsAny<string>()));


            _sut.CreateConnection(It.IsAny<string>(), student);

            _networkRepositoryMock.VerifyAll();
            overlayNetworkMock.VerifyAll();
        }

        [TestMethod()]
        public void SuspendConnectionTest()
        {
            string groupId = "46a2e969-c558-4892-8e45-9cc2875b8268";
            Student student = new()
            {
                Name = "Name",
                IpAddress = "255.255.255.255"
            };

            _networkRepositoryMock.Setup(nr => nr.GetOverlayNetwork(groupId)).Returns(_overlayNetworkMock.Object);
            _overlayNetworkMock.Setup(n => n.RemoveClient(It.IsAny<Student>()));
            _overlayNetworkMock.SetupGet(n => n.OpenVirtualSwitch).Returns(_openVirtualSwitchMock.Object);
            _openVirtualSwitchMock.SetupGet(sw => sw.Key).Returns(It.IsAny<string>());
            _firewallRepositoryMock.Setup(fr => fr.GetFirewall(It.IsAny<string>())).Returns(_firewallMock.Object);
            _firewallMock.Setup(fw => fw.RemoveException(It.IsAny<string>()));
            _networkRepositoryMock.Setup(nr => nr.SaveOverlayNetwork(It.IsAny<IOverlayNetwork>()));


            _sut.SuspendConnection(groupId, student);

            _networkRepositoryMock.VerifyAll();
            _overlayNetworkMock.VerifyAll();
            _openVirtualSwitchMock.VerifyAll();
            _firewallRepositoryMock.VerifyAll();
            _firewallMock.VerifyAll();
            _networkRepositoryMock.VerifyAll();

        }
    }
}