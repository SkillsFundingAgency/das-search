using Specflow_Selenium_PO_Example2.Pages;
using System;
using TechTalk.SpecFlow;

namespace Specflow_Selenium_PO_Example2.Step_Definitions
{
    [Binding]
    public class ExampleWebFeatureSteps : Base
    {
        LoginPage login;

        [Given(@"I have entered username '(.*)' and password '(.*)'")]
        public void GivenIHaveEnteredUsernameAndPassword(string username, string password)
        {
            login = new LoginPage();
            login.with(username, password);
        }

        [When(@"I login")]
        public void WhenILogin()
        {
            login.submit();
        }


        [Then(@"I should be informed that login '(.*)'")]
        public void ThenIShouldBeInformedThatLogin(String status)
        {
            switch (status)
            {
                case "passed":
                    login.successMessagePresent();
                    break;
                case "failed":
                    login.failureMessagePresent();
                    break;
            }
        }
    }
}
