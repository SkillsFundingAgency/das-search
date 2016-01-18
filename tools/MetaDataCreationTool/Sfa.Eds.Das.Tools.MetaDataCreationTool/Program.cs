using CsQuery;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace Sfa.Eds.Das.Tools.MetaDataCreationTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var mappings = LoadMappingFile();

            for (int i = 0; i < mappings.Count(); i++)
            {
                var standardCode = i + 1;
                var pageUrl = mappings[i];

                var standardPageDom = CQ.CreateFromUrl(pageUrl);
                var pdfLink = standardPageDom[".attachment-details > h2 > a"][0];

                CreateFiles(standardCode, pageUrl, pdfLink);
            }

            Console.ReadLine();
        }

        private static IList<string> LoadMappingFile()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            var binDirectory = Path.GetDirectoryName(path);

            var filePath = binDirectory + @"\MappingLinks.txt";

            string[] lines = File.ReadAllLines(filePath);

            return lines;
        }

        private static void CreateFiles(int standardCode, string standardsPagesHref, IDomObject pdfLink)
        {
            var filename = Path.GetInvalidFileNameChars().Aggregate(pdfLink.InnerText.Trim(), (current, c) => current.Replace(c, '_'));
            var filenameWithPrefix = $"{standardCode}-{filename}";
            //DownloadFile(filenameWithPrefix, "https://www.gov.uk" + pdfLink.Attributes["href"]);

            var larsData = GetLarsData(standardCode);

            CreateJsonFile(larsData, pdfLink.InnerText.Trim(), "https://www.gov.uk" + pdfLink.Attributes["href"], standardsPagesHref, filenameWithPrefix);
        }

        private static LarsData GetLarsData(int standardCode)
        {
            var data = DedsClient.GetLarsData(standardCode);

            if (data[1].Results.Length == 0)
            {
                Console.WriteLine($"Couldn't get any data for standard code: {standardCode}");
                return new LarsData { StandardCode = standardCode, Title = string.Empty, NotionalEndLevel = 0 };
            }

            return new LarsData
            {
                StandardCode = standardCode,
                Title = data[1].Results[0][2],
                NotionalEndLevel = Int32.Parse(data[1].Results[0][5])
            };

        }

        private static void CreateJsonFile(LarsData larsData, string title, string pdfUrl, string pageUrl, string pdfFilename)
        {
            Console.WriteLine($"StandardCode: {larsData.StandardCode}, Title: {larsData.Title}, PdfUrl: {pdfUrl}, NotionalEndLevel: {larsData.NotionalEndLevel}");
            Standard standard = new Standard()
            {
                Id = larsData.StandardCode,
                Title = larsData.Title,
                Pdf = new Uri(pdfUrl),
                NotionalEndLevel = larsData.NotionalEndLevel,
                PdfFileName = pdfFilename
            };

            string json = JsonConvert.SerializeObject(standard, Formatting.Indented);
            title = Path.GetInvalidFileNameChars().Aggregate(title, (current, c) => current.Replace(c, '_'));

            if (!Directory.Exists("data\\standards\\json"))
            {
                Directory.CreateDirectory("data\\standards\\json");
            }

            File.WriteAllText($"data\\standards\\json\\{larsData.StandardCode}. {title}.json", json);
        }

        private static void DownloadFile(string filename, string pdfLink)
        {
            var client = new WebClient();

            if (!Directory.Exists("data\\standards\\files"))
            {
                Directory.CreateDirectory("data\\standards\\files");
            }

            client.DownloadFile(pdfLink, "data\\standards\\files\\" + filename + ".pdf");
            client.Dispose();
        }
    }
}
