namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.interfaces;

    public class CsvService : ICsvService
    {
        private readonly IAngleSharpService _angelService;
        private readonly ISettings _settings;

        public CsvService(IAngleSharpService angelService, ISettings settings)
        {
            _angelService = angelService;
            _settings = settings;
        }

        public List<Standard> GetAllStandardsFromCsv(string csvFile)
        {
            List<Standard> standards;
            using (var reader = new StreamReader(File.OpenRead(csvFile)))
            {
                standards = new List<Standard>();
                reader.ReadLine();
                var i = 0;
                while (!reader.EndOfStream && i < this._settings.MaxStandards)
                {
                    i++;
                    var line = reader.ReadLine();
                    var values = line?.Split(',');
                    Standard standard;
                    if(CreateStandard(values, out standard))
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
            if (values == null || values.Length < 7) return false;
            int standardid = TryParse(values[0]);
            if (standardid >= 0)
            {
                standard = new Standard()
                {
                    Id = standardid,
                    Title = values[2].Replace("\"", ""),
                    NotionalEndLevel = TryParse(values[4]),
                    StandardPdf = GetPdfUri(values[8]),
                    AssessmentPlanPdf = GetPdfUri(values[8]),
                    JobRoles = new List<string>(),
                    Keywords = new List<string>(1),
                    TypicalLength = new TypicalLength {Unit = "m"},
                    IntroductoryText = "",
                    OverviewOfRole = "",
                    EntryRequirements = "",
                    WhatApprenticesWillLearn = "",
                    Qualifications = "",
                    ProfessionalRegistration = ""
                };
                return true;
            }
            return false;
        }

        private Uri GetPdfUri(string s)
        {
            var url = _angelService.GetLinks(s.Replace("\"", ""), ".attachment-details h2 a", "Assessment").FirstOrDefault();
            if (url != null)
            {
                return new Uri($"https://www.gov.uk/{url}");
            }
            return null;
        }

        private int TryParse(string s)
        {
            int i;
            if (int.TryParse(s.Replace("\"", ""), out i))
            {
                return i;
            }
            return -1;
        }
    }
}
