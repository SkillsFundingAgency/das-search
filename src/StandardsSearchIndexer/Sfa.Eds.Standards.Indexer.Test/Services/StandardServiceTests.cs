using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services;

namespace Sfa.Eds.Standards.Indexer.Test.Services
{
    [TestFixture]

    public class StandardServiceTests
    {
        [Test]
        public void should_something()
        {
            // Arrange
            var dedsMock = new Mock<IDedsService>();
            blob
                settings
            var sut = new StandardService();

            // Act
            
            // Assert

        }
    }
}
