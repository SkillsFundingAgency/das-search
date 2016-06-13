using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Web.Collections;
using Sfa.Das.Sas.Web.Controllers;
using Sfa.Das.Sas.Web.Services;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Controllers.ControllerBuilders
{
    using Sfa.Das.Sas.Web.Factories.Interfaces;

    internal class ProviderControllerBuilder
    {
        private readonly IConfigurationSettings _configurationSettings = Mock.Of<IConfigurationSettings>(x => x.SurveyUrl == new Uri("http://test.com"));
        private readonly Mock<IProviderSearchService> _mockProviderSearchService = new Mock<IProviderSearchService>();
        private readonly Mock<ILog> _mockLogger = new Mock<ILog>();
        private readonly Mock<IMappingService> _mockMappingService = new Mock<IMappingService>();
        private readonly Mock<IProviderViewModelFactory> _mockViewModelFactory = new Mock<IProviderViewModelFactory>();
        private readonly Mock<IListCollection<int>> _mockCookie = new Mock<IListCollection<int>>();

        private UrlHelper _url;
        private HttpContextBase _httpContext;

        public static implicit operator ProviderController(ProviderControllerBuilder instance)
        {
            return instance.Build();
        }

        public ProviderController Build()
        {
            var controller = new ProviderController(_mockProviderSearchService.Object, _mockLogger.Object, _mockMappingService.Object, _mockViewModelFactory.Object, _configurationSettings, _mockCookie.Object, new Validation(_mockLogger.Object));

            if (_url != null)
            {
                controller.Url = _url;
            }

            if (_httpContext != null)
            {
                controller.ControllerContext = new ControllerContext(_httpContext, new RouteData(), controller);
            }

            return controller;
        }

        public ProviderControllerBuilder SetupProviderService<TResult>(Expression<Func<IProviderSearchService, TResult>> expression, TResult result)
        {
            _mockProviderSearchService.Setup(expression).Returns(result);

            return this;
        }

        public ProviderControllerBuilder SetupMappingService<TResult>(Expression<Func<IMappingService, TResult>> expression, TResult result)
        {
            _mockMappingService.Setup(expression).Returns(result);

            return this;
        }

        public ProviderControllerBuilder SetupViewModelFactory<TResult>(Expression<Func<IProviderViewModelFactory, TResult>> expression, TResult result)
        {
            _mockViewModelFactory.Setup(expression).Returns(result);

            return this;
        }

        public ProviderControllerBuilder SetupCookieCollection<TResult>(Expression<Func<IListCollection<int>, TResult>> expression, TResult result)
        {
            _mockCookie.Setup(expression).Returns(result);

            return this;
        }

        public ProviderControllerBuilder WithUrl(UrlHelper url)
        {
            _url = url;

            return this;
        }

        public ProviderControllerBuilder WithControllerHttpContext(HttpContextBase context)
        {
            _httpContext = context;

            return this;
        }
    }
}
