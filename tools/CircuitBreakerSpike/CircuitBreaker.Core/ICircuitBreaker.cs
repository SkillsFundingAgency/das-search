namespace CircuitBreaker.Core
{
    using System;

    public interface ICircuitBreaker
    {
        void Execute(Action action);
    }
}