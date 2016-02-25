namespace CircuitBreaker.Tests
{
    using System;
    using System.Net;

    using CircuitBreaker.Core;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class PollyRetryTests
    {
        private Mock<IFoo> mock;
        private Exception exception;
        private PollyRetry sut;

        public Action Do(string url)
        {
            return () =>
            {
                this.mock.Object.Bar();
                new HttpClient().Get(url);
            };
        }

        [SetUp]
        public void Setup()
        {
            this.mock = new Mock<IFoo>();
            this.sut = new PollyRetry();
        }

        [Test]
        [ExpectedException(typeof(WebException))]
        public void should_call_a_500_page_and_retry()
        {
            try
            {
                this.sut.Execute(this.Do("http://httpstat.us/500"));
            }
            catch (Exception ex)
            {
                this.exception = ex;
            }

            this.mock.Verify(x => x.Bar(), Times.Exactly(3));

            if (this.exception != null)
            {
                throw this.exception;
            }
        }
    }
}