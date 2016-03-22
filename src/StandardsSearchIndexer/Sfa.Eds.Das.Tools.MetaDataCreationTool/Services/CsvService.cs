namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Sfa.Eds.Das.Indexer.Core.Extensions;
    using Sfa.Eds.Das.Indexer.Core.Models.Framework;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;

    public class CsvService : IReadStandardsFromCsv
    {
        private readonly IAngleSharpService angelService;

        public CsvService(IAngleSharpService angelService)
        {
            this.angelService = angelService;
        }

        public List<Standard> ReadStandardsFromFile(string csvFilePath)
        {
            List<Standard> standards;
            using (var reader = new StreamReader(File.OpenRead(csvFilePath)))
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

        public List<Standard> ReadStandardsFromStream(string csvFile)
        {
            var standards = new List<Standard>();
            foreach (var line in csvFile.Split('\n'))
            {
                var values = line?.Split(',');
                Standard standard;
                if (CreateStandard(values, out standard))
                {
                    standards.Add(standard);
                }
            }

            return standards;
        }

        public List<FrameworkMetaData> ReadFrameworksFromStream(string csvFile)
        {
            var frameworks = new List<FrameworkMetaData>();
            foreach (var line in csvFile.Split('\n'))
            {
                var values = line?.Split(',');
                FrameworkMetaData framework;
                if (CreateFramework(values, out framework))
                {
                    frameworks.Add(framework);
                }
            }

            return frameworks;
        }

        private bool CreateFramework(string[] values, out FrameworkMetaData framework)
        {
            framework = null;
            if (values.Length > 11)
            {
                framework = new FrameworkMetaData
                                {
                                    FworkCode = TryParse(values[0]),
                                    ProgType = TryParse(values[1]),
                                    PwayCode = TryParse(values[2]),
                                    PathwayName = values[3].RemoveQuotationMark(),
                                    EffectiveFrom = TryGetDate(values[4].RemoveQuotationMark()),
                                    EffectiveTo = TryGetDate(values[5].RemoveQuotationMark()),
                                    IssuingAuthorityTitle = values[11].RemoveQuotationMark(),
                                    NASTitle = values[9].RemoveQuotationMark()
                };
                return framework.FworkCode > 0;
            }

            return false;
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
                    Title = values[2].RemoveQuotationMark(),
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
            var url = angelService.GetLinks(s.RemoveQuotationMark(), ".attachment-details h2 a", "Assessment").FirstOrDefault();
            if (url != null)
            {
                return new Uri($"https://www.gov.uk/{url}").ToString();
            }

            return string.Empty;
        }

        private int TryParse(string s)
        {
            int i;
            if (int.TryParse(s.RemoveQuotationMark(), out i))
            {
                return i;
            }

            return -1;
        }

        private DateTime TryGetDate(string dateString)
        {
            DateTime dateTime;
            if (DateTime.TryParse(dateString, out dateTime))
            {
                return dateTime;
            }

            return DateTime.MinValue;
        }
    }
}
