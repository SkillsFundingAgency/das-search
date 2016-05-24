namespace Sfa.Das.WebTest.QA_Regression
{
    using System;

    using NUnit.Framework;

    using Sfa.Das.WebTest.Infrastructure.Selenium;

    using TechTalk.SpecFlow;

    [Binding]
    public class OnPageRegressionSteps
    {
        [Given(@"I was on the (.*)")]
        [When(@"I am on the (.*) page")]
        [Then(@"I am on the (.*) page")]
        public void IamOnThePage(string page)
        {
            
        }
        

    }

}
