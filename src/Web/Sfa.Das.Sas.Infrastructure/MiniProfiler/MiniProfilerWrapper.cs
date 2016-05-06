namespace Sfa.Das.Sas.Infrastructure.MiniProfiler
{
    using System;

    using Sfa.Das.Sas.ApplicationServices;

    using StackExchange.Profiling;

    public class MiniProfilerWrapper : IProfileAStep
    {
        public IDisposable CreateStep(string name)
        {
            return MiniProfiler.Current.Step(name);
        }
    }
}