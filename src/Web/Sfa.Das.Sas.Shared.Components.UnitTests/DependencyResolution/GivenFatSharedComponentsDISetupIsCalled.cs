using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Http;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Infrastructure.Elasticsearch;
using Sfa.Das.Sas.Shared.Components.DependencyResolution;
using SFA.DAS.Apprenticeships.Api.Client;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sfa.Das.Sas.Shared.Components.UnitTests.DependencyResolution
{
    [TestFixture]
    public class Given_Fat_Shared_Components_DI_Setup_Is_Called
    {

        private IServiceCollection _serviceCollection;
        private Mock<IFatConfigurationSettings> _FatConfiguration;
        private IServiceProvider _serviceProvider;

        private string _fatApiUrl = "http://fatApi.url";

        [SetUp]
        public void Setup()
        {
            _FatConfiguration = new Mock<IFatConfigurationSettings>();

            _FatConfiguration.Setup(s => s.FatApiBaseUrl).Returns(_fatApiUrl);

            _serviceCollection = new ServiceCollection();

            _serviceCollection.AddFatSharedComponents(_FatConfiguration.Object);

            _serviceProvider = _serviceCollection.BuildServiceProvider();

        }

        [Test]
        public void Then_Mediatr_DI_Is_Called()
        {
            _serviceCollection.Where(w => w.ServiceType.FullName.StartsWith("MediatR.IRequestHandler")).Should().HaveCountGreaterThan(1);
        }

        [Test]
        public void Then_StandardApiClient_is_registered()
        {
            var standardApi = _serviceProvider.GetService<IStandardApiClient>();

            standardApi.Should().NotBeNull();
            standardApi.Should().BeOfType<StandardApiClient>();
        }
        [Test]
        public void Then_FrameworkApiClient_is_registered()
        {
            var frameworkApi = _serviceProvider.GetService<IFrameworkApiClient>();

            frameworkApi.Should().NotBeNull();
            frameworkApi.Should().BeOfType<FrameworkApiClient>();
        }

        [Test]
        public void Then_HttpService_is_registered()
        {
            var httpClient = _serviceProvider.GetService<IHttpGet>();

            httpClient.Should().NotBeNull();
            httpClient.Should().BeOfType<HttpService>();
        }

        [Test]
        public void Then_ElasticsearchClientFactory_is_not_registered()
        {
            var elasticsearchClientFactory = _serviceProvider.GetService<IElasticsearchClientFactory>();

            elasticsearchClientFactory.Should().BeNull();
        }
    }
}