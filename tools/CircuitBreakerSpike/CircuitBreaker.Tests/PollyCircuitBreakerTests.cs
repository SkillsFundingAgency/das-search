namespace CircuitBreaker.Tests
{
    using System;
    using System.Threading;

    using CircuitBreaker.Core;

    using Moq;

    using NUnit.Framework;

    using Polly.CircuitBreaker;

    [TestFixture]
    public class PollyCircuitBreakerTests
    {
        private Mock<IFoo> mock;
        private Exception exception;

        [SetUp]
        public void Setup()
        {
            mock = new Mock<IFoo>();
        }

        public Action Do(string url)
        {
            return () =>
                {
                    this.mock.Object.Bar();
                    new HttpClient().Get(url);
                };
        }

        [Test]
        public void should_reset_the_breaker_after_3s()
        {
            var sut = new PollyCircuitBreaker(2, 3);
            for (int i = 0; i < 2; i++)
            {
                try
                {
                    sut.Execute(Do("http://httpstat.us/500"));
                }
                catch (Exception)
                {
                }
            }

            Thread.Sleep(TimeSpan.FromSeconds(3));

            sut.Execute(Do("http://httpstat.us/200"));
            this.mock.Verify(x => x.Bar(), Times.Exactly(3));
        }

        [Test]
        public void should_successfully_call_a_200_page_through_the_breaker()
        {
            var sut = new PollyCircuitBreaker(2, 3);
            sut.Execute(Do("http://httpstat.us/200"));
            this.mock.Verify(x => x.Bar(), Times.Once);
        }

        [Test]
        [ExpectedException(typeof(BrokenCircuitException))]
        public void should_call_a_500_page_twice_then_break_the_circuit()
        {
            var sut = new PollyCircuitBreaker(2, 3);
            var exceptions = 0;
            for (int i = 0; i < 2; i++)
            {
                try
                {
                    sut.Execute(Do("http://httpstat.us/500"));
                }
                catch (Exception)
                {
                    exceptions++;
                }
            }

            try
            {
                sut.Execute(Do("http://httpstat.us/500"));
            }
            catch (Exception ex)
            {
                this.exception = ex;
            }

            Assert.AreEqual(2, exceptions);
            this.mock.Verify(x => x.Bar(), Times.Exactly(2));

            if (this.exception != null)
            {
                throw this.exception;
            }
        }
    }
}
