using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Core;
using NUnit.Framework;
using Sfa.Eds.Das.Web.Models;
using Sfa.Eds.Das.Web.Services;

namespace Sfa.Eds.Das.Web.Tests.Services
{
    [TestFixture]
    public class LocationServiceTests
    {
        [Test]
        public async void ShouldReturnTrueIfLocationIsIntoGb()
        {
            // Arrange
            LocationService sut = new LocationService();

            // Act
            var result = await sut.IsLatLonIntoGb(52.4123823, -1.5372957); // SFA, Coventry, GB

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async void ShouldReturnFalseIfLocationIsNotIntoGb()
        {
            // Arrange
            LocationService sut = new LocationService();

            // Act
            var result = await sut.IsLatLonIntoGb(38.2677604, -0.6964941); // Elche, Spain

            // Assert
            Assert.IsFalse(result);
        }
    }
}
