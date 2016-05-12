namespace Sfa.Das.Sas.Indexer.Infrastructure.UnitTests.CourseDirectory
{
    using System.Linq;

    using Core.Services;

    using FluentAssertions;

    using Infrastructure.CourseDirectory;
    using Infrastructure.Settings;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class CourseDirectoryClientTest
    {
        [Test]
        public void WhenCallingCourseDirectoryClientTest()
        {
            var moqSettings = new Mock<IInfrastructureSettings>();
            moqSettings.Setup(m => m.CourseDirectoryUri).Returns("http://www.fake.url/to.course/directory");

            var moqLogger = new Mock<ILog>();

            var courseDirectoryClient = new CourseDirectoryClient(moqSettings.Object, new Stub.StubCourseDirectoryResponseClient(), moqLogger.Object);

            var providers = courseDirectoryClient.GetApprenticeshipProvidersAsync().Result;

            // ToDo: Assert on logic in courseDiretoryClient. 

            var first = providers.FirstOrDefault();

            providers.Count().Should().Be(6);

            first.Frameworks.Count().Should().Be(6);
            first.Standards.Count().Should().Be(1);
        }
    }
}
