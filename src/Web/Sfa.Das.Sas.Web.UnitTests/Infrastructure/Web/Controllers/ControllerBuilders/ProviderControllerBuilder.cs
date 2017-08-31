namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Controllers.ControllerBuilders
{
    using System;
    using System.Linq.Expressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Core.Configuration;
    using MediatR;
    using Moq;
    using Sas.Web.Controllers;
    using Sas.Web.Services;
    using SFA.DAS.NLog.Logger;

    internal class ProviderControllerBuilder
    {
        private readonly Mock<ILog> _mockLogger = new Mock<ILog>();
        private readonly Mock<IMappingService> _mockMappingService = new Mock<IMappingService>();
        private readonly Mock<IMediator> _mockMediator = new Mock<IMediator>();
        private readonly Mock<IConfigurationSettings> _mockSettings = new Mock<IConfigurationSettings>();

        private UrlHelper _url;
        private HttpContextBase _httpContext;

        public static implicit operator ProviderController(ProviderControllerBuilder instance)
        {
            return instance.Build();
        }

        public ProviderController Build()
        {
            var controller = new ProviderController(_mockLogger.Object, _mockMappingService.Object, _mockMediator.Object, _mockSettings.Object);

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

        public ProviderControllerBuilder SetupMappingService<TResult>(Expression<Func<IMappingService, TResult>> expression, TResult result)
        {
            _mockMappingService.Setup(expression).Returns(result);

            return this;
        }

        public ProviderControllerBuilder SetupMediator<TResult>(Expression<Func<IMediator, TResult>> expression, TResult result)
        {
            _mockMediator.Setup(expression).Returns(result);

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