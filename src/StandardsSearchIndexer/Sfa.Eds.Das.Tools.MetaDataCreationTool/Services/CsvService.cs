namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;

    public class CsvService : IReadStandardsFromCsv
    {
        private readonly IAngleSharpService angelService;
        private readonly IAppServiceSettings appServiceSettings;

        public CsvService(IAngleSharpService angelService, IAppServiceSettings appServiceSettings)
        {
            this.angelService = angelService;
            this.appServiceSettings = appServiceSettings;
        }

        public List<Standard> ReadStandardsFromFile(string csvFile)
        {
            List<Standard> standards;
            using (var reader = new StreamReader(File.OpenRead(csvFile)))
            {
                reader.ReadLine();
                standards = new List<Standard>();

                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine()?.Split(',');
                    Standard standard;
                    if (CreateStandard(values, out standard))
                    {
                        standards.Add(standard);
                    }
                }
            }

            return standards;
        }

        private bool CreateStandard(string[] values, out Standard standard)
        {
            standard = null;
            var standardid = GetStandardId(values);
            if (standardid >= 0)
            {
                standard = new Standard()
                {
                    Id = standardid,
                    Title = values[2].Replace("\"", string.Empty),
                    NotionalEndLevel = TryParse(values[4]),
                    StandardPdfUrl = GetPdfUri(values[8]),
                    AssessmentPlanPdfUrl = GetPdfUri(values[8]),
                    JobRoles = new List<string>(),
                    Keywords = new List<string>(1),
                    TypicalLength = new TypicalLength { Unit = "m" },
                    IntroductoryText = string.Empty,
                    OverviewOfRole = string.Empty,
                    EntryRequirements = string.Empty,
                    WhatApprenticesWillLearn = string.Empty,
                    Qualifications = string.Empty,
                    ProfessionalRegistration = string.Empty
                };
                return true;
            }

            return false;
        }

        private int GetStandardId(string[] values)
        {
            if (values == null || values.Length < 7)
            {
                return -1;
            }

            return TryParse(values[0]);
        }

        private string GetPdfUri(string s)
        {
            var url = angelService.GetLinks(s.Replace("\"", string.Empty), ".attachment-details h2 a", "Assessment").FirstOrDefault();
            if (url != null)
            {
                return new Uri($"https://www.gov.uk/{url}").ToString();
            }

            return string.Empty;
        }

        private int TryParse(string s)
        {
            int i;
            if (int.TryParse(s.Replace("\"", string.Empty), out i))
            {
                return i;
            }

            return -1;
        }
    }
}
