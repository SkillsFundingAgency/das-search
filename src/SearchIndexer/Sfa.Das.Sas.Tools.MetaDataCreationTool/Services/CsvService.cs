using System;
using System.Collections.Generic;
using Sfa.Das.Sas.Indexer.Core.Extensions;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Models;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Services
{
    public class CsvService : IReadStandardsFromCsv
    {
        public List<LarsStandard> ReadStandardsFromStream(string csvFile)
        {
            var standards = new List<LarsStandard>();
            foreach (var line in csvFile.Split('\n'))
            {
                var values = LineValues(line);
                LarsStandard larsStandard;
                if (CreateStandard(values, out larsStandard))
                {
                    standards.Add(larsStandard);
                }
            }

            return standards;
        }

        public List<FrameworkMetaData> ReadFrameworksFromStream(string csvFile)
        {
            var frameworks = new List<FrameworkMetaData>();
            foreach (var line in csvFile.Split('\n'))
            {
                var values = LineValues(line);
                FrameworkMetaData framework;
                if (CreateFramework(values, out framework))
                {
                    frameworks.Add(framework);
                }
            }

            return frameworks;
        }

        private string[] LineValues(string line)
        {
            return line?.Split(new[] { "\",\"" }, StringSplitOptions.None);
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
                    EffectiveFrom = TryGetDate(values[4].RemoveQuotationMark()) ?? DateTime.MinValue,
                    EffectiveTo = TryGetDate(values[5].RemoveQuotationMark()),
                    SectorSubjectAreaTier1 = TryParseDouble(values[6]),
                    SectorSubjectAreaTier2 = TryParseDouble(values[7]),
                    NASTitle = values[9].RemoveQuotationMark()
                };
                return framework.FworkCode > 0;
            }

            return false;
        }

        private bool CreateStandard(string[] values, out LarsStandard larsStandard)
        {
            larsStandard = null;
            var standardid = GetStandardId(values);
            if (standardid >= 0)
            {
                larsStandard = new LarsStandard
                {
                    Id = standardid,
                    Title = values[2].RemoveQuotationMark(),
                    NotionalEndLevel = TryParse(values[4]),
                    StandardUrl = values[8],
                    SectorSubjectAreaTier1 = TryParseDouble(values[9]),
                    SectorSubjectAreaTier2 = TryParseDouble(values[10]),
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

        private int TryParse(string s)
        {
            int i;
            if (int.TryParse(s.RemoveQuotationMark(), out i))
            {
                return i;
            }

            return -1;
        }

        private double TryParseDouble(string s)
        {
            double i;
            if (double.TryParse(s.RemoveQuotationMark(), out i))
            {
                return i;
            }

            return -1;
        }

        private DateTime? TryGetDate(string dateString)
        {
            DateTime dateTime;
            if (DateTime.TryParse(dateString, out dateTime))
            {
                return dateTime;
            }

            return null;
        }
    }
}
