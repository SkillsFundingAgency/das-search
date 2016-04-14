namespace Sfa.Das.WebTest.Infrastructure.Hooks
{
    using System;

    using BoDi;

    using OpenQA.Selenium;

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
            this._objectContainer = objectContainer;

            _objectContainer.RegisterTypeAs<BrowserSettings, IBrowserSettings>();
            _objectContainer.RegisterTypeAs<PageContext, IPageContext>();
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            DeploymentValidator.WaitForDeployment();
        }

        [BeforeFeature]
        public static void BeforeFeature()
        {
            Console.WriteLine($"#####################  Feature: {FeatureContext.Current.FeatureInfo.Title}  ######################");
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            seleniumContext = new SeleniumContext();
            _objectContainer.RegisterInstanceAs<IWebDriver>(seleniumContext.WebDriver);
            
            Console.WriteLine("##### Scenario: " + ScenarioContext.Current.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if (ScenarioContext.Current.TestError != null)
            {
                seleniumContext.WebDriver.TakeScreenshot();
            }

            seleniumContext.Dispose();
        }

        [AfterFeature]
        public static void AfterFeature()
        {
        }
    }
}