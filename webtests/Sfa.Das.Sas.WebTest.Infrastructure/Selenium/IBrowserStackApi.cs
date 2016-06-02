namespace Sfa.Das.Sas.WebTest.Infrastructure.Selenium
{
    using System;

    public interface IBrowserStackApi
    {
        void FailTestSession(Exception testError);
    }
}