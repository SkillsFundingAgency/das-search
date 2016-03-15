namespace Sfa.Infrastructure.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using LINQtoCSV;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure;
    using Sfa.Eds.Das.Indexer.Core.Extensions;

    public class CsvService : IConvertFromCsv
    {
        public ICollection<T> CsvTo<T>(string result)
            where T : class, new()
        {
            var cc = new CsvContext();
            var stream = result.GenerateStreamFromString();
            var reader = new StreamReader(stream);
            return cc.Read<T>(reader).ToList();
        }
    }
}