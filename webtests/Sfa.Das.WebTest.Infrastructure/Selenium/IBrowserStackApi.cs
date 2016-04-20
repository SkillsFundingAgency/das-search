namespace Sfa.Das.WebTest.Infrastructure.Selenium
{
    using System;

    public interface IBrowserStackApi
    {
        void FailTestSession(string reason);

        void FailTestSession(Exception testError);
    }
}