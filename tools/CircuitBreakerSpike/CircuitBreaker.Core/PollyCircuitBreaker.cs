namespace CircuitBreaker.Core
{
    using System;
    using Polly;
    using Polly.CircuitBreaker;

    public class PollyCircuitBreaker : ICircuitBreaker
    {
        private CircuitBreakerPolicy policy;

        public PollyCircuitBreaker(int breakOnNumberOfExceptions, int breakCircuitForSeconds)
        {
            policy = Policy.Handle<Exception>()
                .CircuitBreaker(breakOnNumberOfExceptions, TimeSpan.FromSeconds(breakCircuitForSeconds));
        }

        public void Execute(Action action)
        {
            policy.Execute(action);
        }
    }
}