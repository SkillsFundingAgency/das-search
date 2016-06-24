using System;
using AutoMapper;

namespace Sfa.Das.Sas.Web.Services
{
    public interface IMappingService
    {
        TDest Map<TSource, TDest>(TSource source);

        TDest Map<TSource, TDest>(TSource source, Action<IMappingOperationOptions<TSource, TDest>> opts);
    }
}