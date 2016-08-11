using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sfa.Das.Sas.Core.Logging;

namespace Sfa.Das.Sas.ApplicationServices.Services
{
    public class JsonMetaDataConvert : IJsonMetaDataConvert
    {
        //private readonly ILog _logger;

        public JsonMetaDataConvert(/*ILog logger*/)
        {
            //this._logger = logger;
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
                    //_logger.Warn(ex, $"Couldn't deserialise {typeof(T)} meta data for: {item.Key}");
                }
            }

            return jsonData;
        }
    }
}
