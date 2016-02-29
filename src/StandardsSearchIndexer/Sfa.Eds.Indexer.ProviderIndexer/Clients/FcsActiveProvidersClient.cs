namespace Sfa.Eds.Das.ProviderIndexer.Clients
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using LINQtoCSV;

    using Sfa.Eds.Das.ProviderIndexer.Models;
    using Sfa.Eds.Das.ProviderIndexer.Settings;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;

    public class FcsActiveProvidersClient : IActiveProviderClient
    {
        private readonly IVstsClient _vstsClient;

        private readonly IProviderIndexSettings _settings;

        public FcsActiveProvidersClient(IVstsClient vstsClient, IProviderIndexSettings settings)
        {
            _vstsClient = vstsClient;
            _settings = settings;
        }

        public async Task<IEnumerable<string>> GetProviders()
        {
            var result = _vstsClient.GetFileContent(_settings.ActiveProvidersPath);
            var cc = new CsvContext();
            var stream = GenerateStreamFromString(result);
            var reader = new StreamReader(stream);
            var records = cc.Read<FcsProviderRecord>(reader).ToList();
            return records.Select(x => x.UkPrn);
        }

        public Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}