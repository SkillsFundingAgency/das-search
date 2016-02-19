namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services
{
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.interfaces;

    public class FileService : IFileService
    {
        public void CreateJsonFile(Standard standard, string path, bool overwrite = false)
        {

            string json = JsonConvert.SerializeObject(standard, Formatting.Indented);
            var pdfName = Path.GetInvalidFileNameChars().Aggregate(standard.Title, (current, c) => current.Replace(c, '_')).Replace(" ", "");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var fileName = $"{path}\\{standard.Id}-{pdfName}.json";

            if (overwrite || !File.Exists(fileName))
            {
                File.WriteAllText(fileName, json);
            }
        }

        public void UpdateJsonFile(Standard standard, string path)
        {
            var pdfName = Path.GetInvalidFileNameChars().Aggregate(standard.Title, (current, c) => current.Replace(c, '_')).Replace(" ", "");
            var fileName = $"{path}\\{standard.Id}-{pdfName}.json";

            var oldStandard = GetStandard(fileName);

            standard = MergeStandards(standard, oldStandard);

            string json = JsonConvert.SerializeObject(standard, Formatting.Indented);
            

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.WriteAllText(fileName, json); 
        }

        private Standard MergeStandards(Standard standard, Standard oldStandard)
        {
            if (oldStandard == null) return standard;

            return new Standard
            {
                Id = standard.Id,
                AssessmentPlanPdf = oldStandard.AssessmentPlanPdf ?? standard.AssessmentPlanPdf,
                StandardPdf = oldStandard.StandardPdf ?? standard.StandardPdf,
                JobRoles = oldStandard.JobRoles,
                Keywords = oldStandard.Keywords,
                TypicalLength = oldStandard.TypicalLength,
                IntroductoryText = oldStandard.IntroductoryText,
                OverviewOfRole = oldStandard.OverviewOfRole,
                EntryRequirements = oldStandard.EntryRequirements,
                WhatApprenticesWillLearn = oldStandard.WhatApprenticesWillLearn,
                Qualifications = oldStandard.Qualifications,
                ProfessionalRegistration = oldStandard.ProfessionalRegistration
            };
        }

        private Standard GetStandard(string fileName)
        {
            var reader = new StreamReader(File.OpenRead(fileName));
            var standard = JsonConvert.DeserializeObject<Standard>(reader.ReadToEnd());
            return standard;
        }
    }
}
