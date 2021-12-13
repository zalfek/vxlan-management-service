﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

namespace OverlayManagementService.Services.Tests
{
    [TestClass()]
    public class VMOverlayConnectionServiceTests
    {
        private readonly VMOverlayConnectionService _sut;
        private readonly Mock<IRepository> _jsonRepositoryMock = new Mock<IRepository>();
        private readonly Mock<ILogger<VMOverlayConnectionService>> _loggerMock = new Mock<ILogger<VMOverlayConnectionService>>();
        private readonly Mock<IAddress> _ipAddressMock = new Mock<IAddress>();
        private readonly Mock<IClientConnectionFactory> _clientConnectionFactoryMock = new Mock<IClientConnectionFactory>();

        public VMOverlayConnectionServiceTests()
        {
            _sut = new VMOverlayConnectionService(_jsonRepositoryMock.Object, _loggerMock.Object, _ipAddressMock.Object, _clientConnectionFactoryMock.Object);
        }

        [TestMethod()]
        public void GetAllNetworksTest()
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("group", "adggjdasd45t54zuw46us"));
            claims.Add(new Claim("group", "adgasdajkjdjghzuw46us"));
            claims.Add(new Claim("group", "adgsdfgaz5645f90mmsds"));

            ClientConnection clientConnection = new ClientConnection("1", "adggjdasd45t54zuw46us", "255.255.255.255", "255.255.255.255");
            Mock<IOverlayNetwork> overlayNetworkMock = new Mock<IOverlayNetwork>();
            _jsonRepositoryMock.Setup(x => x.GetOverlayNetwork(It.IsAny<Claim>())).Returns(overlayNetworkMock.Object);
            _clientConnectionFactoryMock.Setup(c => c.CreateClientConnectionDto(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(clientConnection);
            overlayNetworkMock.SetupGet(x => x.VNI).Returns("1");
            overlayNetworkMock.SetupGet(x => x.OpenVirtualSwitch.PublicIP).Returns("255.255.255.255");

            _sut.GetAllNetworks(claims);

            _jsonRepositoryMock.VerifyAll();
            _clientConnectionFactoryMock.VerifyAll();
        }

        [TestMethod()]
        public void CreateConnectionTest()
        {
            Mock<IOverlayNetwork> overlayNetworkMock = new Mock<IOverlayNetwork>();
            _jsonRepositoryMock.Setup(x => x.GetOverlayNetwork(It.IsAny<string>())).Returns(overlayNetworkMock.Object);
            overlayNetworkMock.Setup(n => n.AddClient(It.IsAny<string>()));
            ClientConnection clientConnection = new ClientConnection("1", "adggjdasd45t54zuw46us", "255.255.255.255", "255.255.255.255");
            _clientConnectionFactoryMock.Setup(c => c.CreateClientConnectionDto(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(clientConnection);
            overlayNetworkMock.SetupGet(x => x.VNI).Returns("1");
            overlayNetworkMock.SetupGet(x => x.OpenVirtualSwitch.PublicIP).Returns("255.255.255.255");


            _sut.CreateConnection(It.IsAny<string>(), It.IsAny<string>());

            _jsonRepositoryMock.VerifyAll();
            overlayNetworkMock.VerifyAll();
        }
    }
}