using Microsoft.VisualStudio.TestTools.UnitTesting;
using OverlayManagementService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using OverlayManagementService.Resolvers;
using OverlayManagementService.DataTransferObjects;
using OverlayManagementService.Repositories;
using OverlayManagementService.Network;
using Microsoft.Extensions.Logging;

namespace OverlayManagementService.Services.Tests
{
    [TestClass()]
    public class VMOverlayConnectionServiceTests
    {
        private readonly VMOverlayConnectionService _sut;
        private readonly Mock<IMembershipResolver> _membershipResolverMock = new Mock<IMembershipResolver>();
        private readonly Mock<IRepository> _jsonRepositoryMock = new Mock<IRepository>();
        private readonly Mock<ILogger<VMOverlayConnectionService>> _loggerMock = new Mock<ILogger<VMOverlayConnectionService>>();

        public VMOverlayConnectionServiceTests()
        {
            _sut = new VMOverlayConnectionService(_membershipResolverMock.Object, _jsonRepositoryMock.Object, _loggerMock.Object);
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

            _jsonRepositoryMock.Setup(x => x.GetOverlayNetwork(membership)).Returns(new VXLANOverlayNetwork());

            //Act
            var result = _sut.GetOverlayNetwork(membership);

            //Assert
            _jsonRepositoryMock.Verify(r => r.GetOverlayNetwork(membership), Times.Once());
        }

        [TestMethod()]
        public void GetAllMembershipsTest()
        {
            //Arrange
            var user = new Student(
                "John",
                "Doe",
                "john.doe@hs-ulm.de",
                "537faa0c-9461-4be0-85cb-87fcb4105881",
               "255.255.255.255"
               );

            _membershipResolverMock.Setup(x => x.GetUserMemberships(user)).Returns(Task.FromResult(new List<IMembership>()));

            //Act
            var result = _sut.GetUserMemberships(user);

            //Assert
            _membershipResolverMock.Verify(s => s.GetUserMemberships(user), Times.Once());

        }
    }
}