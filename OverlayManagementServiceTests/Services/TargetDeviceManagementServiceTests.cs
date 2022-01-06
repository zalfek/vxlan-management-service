using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OverlayManagementService.Dtos;
using OverlayManagementService.Factories;
using OverlayManagementService.Network;
using OverlayManagementService.Repositories;
using OverlayManagementService.Services;
using System;

namespace OverlayManagementService.Services.Tests
{
    [TestClass()]
    public class TargetDeviceManagementServiceTests
    {
        private readonly TargetDeviceManagementService _sut;
        private readonly Mock<INetworkRepository> _networkRepositorMock = new();
        private readonly Mock<ILogger<TargetDeviceManagementService>> _loggerMock = new();
        private readonly Mock<IAddress> _ipAddressMock = new();
        private readonly Mock<IOpenVirtualSwitch> _openVirtualSwitchMock = new();
        private readonly Mock<IOverlayNetwork> _overlayNetworkMock = new();
        private readonly Mock<IVirtualMachineFactory> _virtualMachineFactoryMock = new();
        private readonly Mock<IVirtualMachine> _virtualMachineMock = new();
        private readonly Mock<IKeyKeeper> _KeyKeeperMock = new();
        private readonly Mock<IConfiguration> _configurationMock = new();

        public TargetDeviceManagementServiceTests()
        {
            _sut = new TargetDeviceManagementService(
                _loggerMock.Object,
                _networkRepositorMock.Object,
                _virtualMachineFactoryMock.Object,
                _configurationMock.Object,
                _KeyKeeperMock.Object  
                );
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
    }
}