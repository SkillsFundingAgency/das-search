using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace LatLongLocator.Test
{
    [TestFixture]
    public class LocationServiceTests
    {
        [Test]
        public async Task ShouldReturnTrueIfLocationIsIntoGb()
        {
            // Arrange
            LocationService sut = new LocationService();

            // Act
            var result = await sut.IsLatLonIntoGb(52.4123823, -1.5372957); // SFA, Coventry, GB

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ShouldReturnFalseIfLocationIsNotIntoGb()
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
