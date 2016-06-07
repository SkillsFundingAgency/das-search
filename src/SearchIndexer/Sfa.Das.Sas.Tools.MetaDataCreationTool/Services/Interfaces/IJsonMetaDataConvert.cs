namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces
{
    using System.Collections.Generic;

    public interface IJsonMetaDataConvert
    {
        List<T> DeserializeObject<T>(IDictionary<string, string> data);
    }
}