namespace Sfa.Das.Sas.WebTest.Infrastructure.Hooks
{
    using BoDi;

    using Sfa.Das.Sas.WebTest.Infrastructure.Selenium;
    using Sfa.Das.Sas.WebTest.Infrastructure.Services;

    public class IoC
    {
        public static IObjectContainer Initialise(IObjectContainer objectContainer)
        {
            objectContainer.RegisterTypeAs<BrowserStackApi, IBrowserStackApi>();
            objectContainer.RegisterTypeAs<WebRequestRetryService, IRetryWebRequests>();
            objectContainer.RegisterTypeAs<ConsoleLogger, ILog>();
            return objectContainer;
        }
    }
}