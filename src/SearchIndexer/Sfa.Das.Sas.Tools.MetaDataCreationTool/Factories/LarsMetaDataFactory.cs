using System;
using System.Collections.Generic;
using System.Linq;
using Sfa.Das.Sas.Indexer.Core.Extensions;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Models;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories
{
    public class LarsMetaDataFactory : IMetaDataFactory
    {
        public T Create<T>(IEnumerable<string> values)
            where T : class
        {
            if (typeof(T) == typeof(FrameworkMetaData))
            {
                return CreateFrameworkMetaData(values) as T;
            }

            if (typeof(T) == typeof(LarsStandard))
            {
                return CreateStandard(values) as T;
            }

            return null;
        }

        private FrameworkMetaData CreateFrameworkMetaData(IEnumerable<string> frameworkValues)
        {
            var values = frameworkValues.ToList();

            if (values.Count <= 11)
            {
                return null;
            }

            var frameworkCode = TryParse(values[0]);

            if (frameworkCode <= 0)
            {
                return null;
            }

            return new FrameworkMetaData
                {
                    FworkCode = frameworkCode,
                    ProgType = TryParse(values[1]),
                    PwayCode = TryParse(values[2]),
                    PathwayName = values[3].RemoveQuotationMark(),
                    EffectiveFrom = TryGetDate(values[4].RemoveQuotationMark()) ?? DateTime.MinValue,
                    EffectiveTo = TryGetDate(values[5].RemoveQuotationMark()),
                    SectorSubjectAreaTier1 = TryParseDouble(values[6]),
                    SectorSubjectAreaTier2 = TryParseDouble(values[7]),
                    NASTitle = values[9].RemoveQuotationMark()
                };
        }

        private LarsStandard CreateStandard(IEnumerable<string> standardValues)
        {
            var values = standardValues.ToList();

            var standardid = GetStandardId(values);

            if (standardid < 0)
            {
                return null;
            }

            return new LarsStandard
            {
                Id = standardid,
                Title = values[2].RemoveQuotationMark(),
                NotionalEndLevel = TryParse(values[4]),
                StandardUrl = values[8],
                SectorSubjectAreaTier1 = TryParseDouble(values[9]),
                SectorSubjectAreaTier2 = TryParseDouble(values[10]),
            };
        }

        private int GetStandardId(IReadOnlyList<string> values)
        {
            if (values == null || values.Count < 7)
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
