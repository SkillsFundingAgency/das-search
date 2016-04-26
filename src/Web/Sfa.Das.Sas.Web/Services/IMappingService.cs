namespace Sfa.Das.Sas.Web.Services
{
    public interface IMappingService
    {
        TDest Map<TSource, TDest>(TSource source);
    }
}