using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using LARSMetaDataExplorer.CSV;
using LARSMetaDataExplorer.Models;
using LARSMetaDataExplorer.Serialization;
using LARSMetaDataExplorer.Settings;

namespace LARSMetaDataExplorer.Services
{
    public class MetaDataService : IDisposable
    {
        private Stream _stream;
        private readonly ICsvService _csvService;

        public MetaDataService(Stream stream, ICsvService csvService)
        {
            _stream = stream;
            _csvService = csvService;
        }

        public MetaDataBag GetMetaDataBag(AppSettings appSettings)
        {
            MetaDataBag bag;

            Console.WriteLine("Getting metadata bag...");

            if (!File.Exists(appSettings.MetaDataBagFilePath))
            {
                Console.WriteLine("No stored bag found so retrieving meta data from LARS...");
                var timer = Stopwatch.StartNew();

                bag = new MetaDataBag
                {
                    Frameworks = GetMetaData<FrameworkMetaData>(appSettings.CsvFileNameFrameworks),
                    Standards = GetMetaData<LarsStandard>(appSettings.CsvFileNameStandards),
                    FrameworkAims =
                        GetMetaData<FrameworkAimMetaData>(appSettings.CsvFileNameFrameworkAims),
                    FrameworkComponentTypes =
                        GetMetaData<FrameworkComponentTypeMetaData>(
                            appSettings.CsvFileNameFrameworkComponentType),
                    LearningDeliveries =
                        GetMetaData<LearningDeliveryMetaData>(
                            appSettings.CsvFileNameLearningDelivery),
                    Fundings = GetMetaData<FundingMetaData>(appSettings.CsvFileNameFunding)
                };

                Console.WriteLine($"Time taken to get meta data from LARS: {timer.Elapsed.ToString("g")}");

                Console.WriteLine("Serializing metadata bag to file...");
                FileSerializer.SerializeToFile(appSettings.MetaDataBagFilePath, bag);
            }
            else
            {
                Console.WriteLine("Grabbing metadata bag from stored file...");
                bag = FileSerializer.DeserializeFromFile<MetaDataBag>(appSettings.MetaDataBagFilePath);
            }

            return bag;
        }

        public ICollection<T> GetMetaData<T>(string csvFilePath) where T : class
        {
            var fileContent = ExtractFileFromStream(csvFilePath);

            return _csvService.ReadFromString<T>(fileContent);
        }

       
        private string ExtractFileFromStream(string filePath)
        {
            using (var zip = new ZipArchive(_stream, ZipArchiveMode.Read, true))
            {
                var entry = zip.Entries.FirstOrDefault(m => m.FullName.EndsWith(filePath));

                if (entry == null)
                {
                    return null;
                }

                using (var reader = new StreamReader(entry.Open()))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public void Dispose()
        {
            if (_stream == null) return;

            _stream.Dispose();
            _stream = null;
        }
    }
}
