using System.Collections.Generic;

namespace Sfa.Das.Sas.ApplicationServices.Services
{
    public interface IJsonMetaDataConvert
    {
        List<T> DeserializeObject<T>(IDictionary<string, string> data);
    }
}