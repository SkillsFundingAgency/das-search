namespace Sfa.Eds.Das.Tools.MetaDataCreationTool
{
    using System.IO;

    using Newtonsoft.Json;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.interfaces;

    public interface IJsonStringFormater
    {
        void StartStatic();
    }

    public class JsonStringFormater : IJsonStringFormater
    {
        private readonly IFileService fileService;

        public JsonStringFormater(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public void StartStatic()
        {
            var jsonsPath = @"C:\Temp\data\standards\json";
            var files = Directory.GetFiles(jsonsPath);
            foreach (var file in files)
            {
                var standard = LoadStandard(file);
                if (standard.Id == 1)
                {
                    //standard.IntroductoryText = MakeJsonSafe(standard.IntroductoryText);
                    var html = this.ToHtml(standard.IntroductoryText);
                    standard.IntroductoryText = MakeJsonSafe(html);
                    this.fileService.CreateJsonFile(standard, @"C:\Temp\formatedDataHtml", true);
                }
            }
        }

        private string MakeJsonSafe(string text)
        {
            return text.Replace(@"\", @"\\");
        }

        private string ToHtml(string markdownText)
        {
            return CommonMark.CommonMarkConverter.Convert(markdownText);
        }

        private Standard LoadStandard(string filePath)
        {
            var reader = new StreamReader(File.OpenRead(filePath));
            var standard = JsonConvert.DeserializeObject<Standard>(reader.ReadToEnd());
            return standard;
        }
    }
}
