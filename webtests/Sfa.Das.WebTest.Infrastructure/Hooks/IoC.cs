namespace Sfa.Das.WebTest.Infrastructure.Hooks
{
    using BoDi;

    using Sfa.Das.WebTest.Infrastructure.Selenium;
    using Sfa.Das.WebTest.Infrastructure.Services;
    using Sfa.Das.WebTest.Infrastructure.Settings;

    public class IoC
    {
        public static IObjectContainer Initialise(IObjectContainer objectContainer)
        {
            objectContainer.RegisterTypeAs<BrowserSettings, IBrowserSettings>();
            objectContainer.RegisterTypeAs<PageContext, IPageContext>();
            objectContainer.RegisterTypeAs<BrowserStackApi, IBrowserStackApi>();
            objectContainer.RegisterInstanceAs<IProvideSettings>(new AppConfigSettingsProvider(new MachineSettings()));
            objectContainer.RegisterTypeAs<WebRequestRetryService, IRetryWebRequests>();
            objectContainer.RegisterTypeAs<ConsoleLogger, ILog>();

            return objectContainer;
        }
    }
}