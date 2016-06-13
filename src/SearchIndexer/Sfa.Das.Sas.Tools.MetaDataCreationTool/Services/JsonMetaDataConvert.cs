using Sfa.Das.Sas.Indexer.Core.Logging;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Services
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces;

    public class JsonMetaDataConvert : IJsonMetaDataConvert
    {
        private readonly ILog _logger;

        public JsonMetaDataConvert(ILog logger)
        {
            this._logger = logger;
        }

        public List<T> DeserializeObject<T>(IDictionary<string, string> data)
        {
            var jsonData = new List<T>();
            foreach (var item in data)
            {
                try
                {
                    jsonData.Add(JsonConvert.DeserializeObject<T>(item.Value));
                }
                catch (JsonReaderException ex)
                {
                    _logger.Warn(ex, $"Couldn't deserialise {typeof(T)} meta data for: {item.Key}");
                }
            }

            return jsonData;
        }
    }
}
