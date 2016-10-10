namespace Sfa.Das.Sas.ApplicationServices.Logging
{
    using Sfa.Das.Sas.Core.Logging;

    public class HttpErrorLogEntry : ILogEntry
    {
        public string Url { get; set; }
    }
}