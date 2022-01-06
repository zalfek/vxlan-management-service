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
    public class SwitchManagementServiceTests
    {
        private readonly SwitchManagementService _sut;
        private readonly Mock<INetworkRepository> _networkRepositorMock = new();
        private readonly Mock<ISwitchRepository> _switchRepositoryMock = new();
        private readonly Mock<ILogger<SwitchManagementService>> _loggerMock = new();
        private readonly Mock<IOpenVirtualSwitch> _openVirtualSwitchMock = new();
        private readonly Mock<IOverlayNetwork> _overlayNetworkMock = new();
        private readonly Mock<IFirewallFactory> _firewallFactoryMock = new();
        private readonly Mock<IFirewallRepository> _firewallRepositoryMock = new();
        private readonly Mock<IKeyKeeper> _KeyKeeperMock = new();
        private readonly Mock<IOpenVirtualSwitchFactory> _openVirtualSwitchFactoryMock = new();
        private readonly Mock<IFirewall> _firewallMock = new();



        public SwitchManagementServiceTests()
        {
            _sut = new SwitchManagementService(
                _loggerMock.Object,
                _networkRepositorMock.Object,
                _switchRepositoryMock.Object,
                _firewallRepositoryMock.Object,
                _firewallFactoryMock.Object,
                _openVirtualSwitchFactoryMock.Object,
                _KeyKeeperMock.Object
                );
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
    }
}