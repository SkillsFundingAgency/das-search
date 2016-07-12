namespace Sfa.Das.Sas.ApplicationServices
{
    using System;
    public interface IProfileAStep
    {
        IDisposable CreateStep(string name);
    }
}