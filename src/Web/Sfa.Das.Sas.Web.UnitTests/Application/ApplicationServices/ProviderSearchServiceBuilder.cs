using System;
using System.Linq.Expressions;
using Moq;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.Core.Domain.Services;
using SFA.DAS.NLog.Logger;

namespace Sfa.Das.Sas.Web.UnitTests.Application.ApplicationServices
{
    internal sealed class ProviderSearchServiceBuilder
    {
        private readonly Mock<IGetStandards> _standardsRepository = new Mock<IGetStandards>();
        private readonly Mock<IGetFrameworks> _frameworksRepository = new Mock<IGetFrameworks>();
        private readonly Mock<ILog> _logger = new Mock<ILog>();
        private readonly Mock<IPaginationSettings> _paginationSettings = new Mock<IPaginationSettings>();

        internal Mock<IProviderLocationSearchProvider> LocationSearchProvider { get; } = new Mock<IProviderLocationSearchProvider>();

        internal Mock<ILookupLocations> LocationLookup { get; } = new Mock<ILookupLocations>();

        public static implicit operator ProviderSearchService(ProviderSearchServiceBuilder instance)
        {
            return instance.Build();
        }

        public ProviderSearchService Build()
        {
            var controller = new ProviderSearchService(LocationSearchProvider.Object, _standardsRepository.Object, _frameworksRepository.Object, LocationLookup.Object, _logger.Object, _paginationSettings.Object);

            return controller;
        }

        internal ProviderSearchServiceBuilder SetupPostCodeLookup<TResult>(Expression<Func<ILookupLocations, TResult>> expression, TResult result)
        {
            LocationLookup.Setup(expression).Returns(result);

            return this;
        }

        internal ProviderSearchServiceBuilder SetupLocationSearchProvider<TResult>(Expression<Func<IProviderLocationSearchProvider, TResult>> expression, TResult result)
        {
            LocationSearchProvider.Setup(expression).Returns(result);

            return this;
        }

        internal ProviderSearchService SetupLocationSearchProviderException<T>(Expression<Func<IProviderLocationSearchProvider, object>> expression)
            where T : Exception, new()
        {
            LocationSearchProvider.Setup(expression).Throws<T>();

            return this;
        }

        internal ProviderSearchServiceBuilder SetupStandardRepository<TResult>(Expression<Func<IGetStandards, TResult>> expression, TResult result)
        {
            _standardsRepository.Setup(expression).Returns(result);

            return this;
        }

        internal ProviderSearchServiceBuilder SetupFrameworkRepository<TResult>(Expression<Func<IGetFrameworks, TResult>> expression, TResult result)
        {
            _frameworksRepository.Setup(expression).Returns(result);

            return this;
        }
    }
}
