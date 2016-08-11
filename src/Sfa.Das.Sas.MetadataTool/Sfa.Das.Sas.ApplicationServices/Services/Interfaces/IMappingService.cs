using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Sfa.Das.Sas.ApplicationServices.Services
{
    public interface IMappingService
    {
        TDest Map<TSource, TDest>(TSource source);

        TDest Map<TSource, TDest>(TSource source, Action<IMappingOperationOptions<TSource, TDest>> opts);
    }
}
