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

namespace OverlayManagementService.Services.Tests
{
    [TestClass()]
    public class VMOverlayConnectionServiceTests
    {
        private readonly VMOverlayConnectionService _sut;
        private readonly Mock<IRepository> _jsonRepositoryMock = new Mock<IRepository>();
        private readonly Mock<ILogger<VMOverlayConnectionService>> _loggerMock = new Mock<ILogger<VMOverlayConnectionService>>();

        public VMOverlayConnectionServiceTests()
        {
            _sut = new VMOverlayConnectionService(_jsonRepositoryMock.Object, _loggerMock.Object);
        }


        [TestMethod()]
        public void GetOverlayNetworkTest()
        {

            //Arrange

            var membership = new Membership(
                "S-1-5-21-3490585887-3336927534-2905204395-41603",
                "Subject",
                Guid.NewGuid()
                );

            _jsonRepositoryMock.Setup(x => x.GetOverlayNetwork(membership.MembershipId)).Returns(new VXLANOverlayNetwork());

            //Act
            var result = _sut.GetOverlayNetwork(membership);

            //Assert
            _jsonRepositoryMock.Verify(r => r.GetOverlayNetwork(membership.MembershipId), Times.Once());
        }
    }
}