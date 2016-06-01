namespace Sfa.Das.Sas.WebTest.Infrastructure.Hooks
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    using BoDi;

    using OpenQA.Selenium;
    using OpenQA.Selenium.PhantomJS;
    using OpenQA.Selenium.Remote;

    using Sfa.Das.Sas.WebTest.Infrastructure.Selenium;
    using Sfa.Das.Sas.WebTest.Infrastructure.Services;

    using SpecBind.BrowserSupport;
    using SpecBind.Helpers;

    using TechTalk.SpecFlow;

    [Binding]
    public sealed class Hooks
    {
        private readonly IObjectContainer _objectContainer;

        public Hooks(IObjectContainer objectContainer)
        {
            this._objectContainer = IoC.Initialise(objectContainer);
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
        }

        [BeforeFeature]
        public static void BeforeFeature()
        {
            Console.WriteLine($"#####################  Feature: {FeatureContext.Current.FeatureInfo.Title}  ######################");
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            Console.WriteLine("##### Scenario: " + ScenarioContext.Current.ScenarioInfo.Title);
            
            var log = _objectContainer.Resolve<ILog>();
            var settings = SettingHelper.GetConfigurationSection().BrowserFactory.Settings;
            foreach (var key in settings.AllKeys.Where(x => !x.StartsWith("browserstack.")))
            {
                if (!string.IsNullOrEmpty(settings[key]?.Value))
                {
                    log.Info($"{key}:\"{settings[key]?.Value}\"");
                }
            }
        }

        [AfterScenario(Order = -10)]
        public void AfterScenario()
        {
            if (ScenarioContext.Current.TestError != null)
            {
                var driver = _objectContainer.Resolve<IBrowser>().Driver();
                if(driver is RemoteWebDriver && !(driver is PhantomJSDriver))
                {
                    _objectContainer.Resolve<IBrowserStackApi>().FailTestSession(ScenarioContext.Current.TestError);
                }
            }
        }

        [AfterFeature]
        public static void AfterFeature()
        {
        }
    }
}