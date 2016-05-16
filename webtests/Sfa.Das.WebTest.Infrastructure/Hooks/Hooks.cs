namespace Sfa.Das.WebTest.Infrastructure.Hooks
{
    using System;

    using BoDi;

    using OpenQA.Selenium;
    using OpenQA.Selenium.PhantomJS;
    using OpenQA.Selenium.Remote;

    using Sfa.Das.WebTest.Infrastructure.Selenium;
    using Sfa.Das.WebTest.Infrastructure.Settings;

    using TechTalk.SpecFlow;

    [Binding]
    public sealed class Hooks
    {
        private readonly IObjectContainer _objectContainer;

        private static SeleniumContext seleniumContext;

        public Hooks(IObjectContainer objectContainer)
        {
            this._objectContainer = IoC.Initialise(objectContainer);
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            // TODO readd this code in sprint 10
            //DeploymentValidator.WaitForDeployment();
        }

        [BeforeFeature]
        public static void BeforeFeature()
        {
            Console.WriteLine($"#####################  Feature: {FeatureContext.Current.FeatureInfo.Title}  ######################");
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            seleniumContext = _objectContainer.Resolve<SeleniumContext>().InitialiseDriver();
            _objectContainer.RegisterInstanceAs<IWebDriver>(seleniumContext.WebDriver);
            
            Console.WriteLine("##### Scenario: " + ScenarioContext.Current.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if (ScenarioContext.Current.TestError != null)
            {
                seleniumContext.WebDriver.TakeScreenshot();
                if(seleniumContext.WebDriver is RemoteWebDriver && !(seleniumContext.WebDriver is PhantomJSDriver))
                {
                    _objectContainer.Resolve<IBrowserStackApi>().FailTestSession(ScenarioContext.Current.TestError);
                }
            }

            seleniumContext.Dispose();
        }

        [AfterFeature]
        public static void AfterFeature()
        {
        }
    }
}