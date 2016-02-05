namespace Sfa.Eds.Das.Web.Services
{
    public interface IMappingService
    {
        TDest Map<TSource, TDest>(TSource source);
    }
}