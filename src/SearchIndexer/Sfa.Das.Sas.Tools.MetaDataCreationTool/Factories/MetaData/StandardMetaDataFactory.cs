using System;
using System.Collections.Generic;
using Sfa.Das.Sas.Indexer.Core.Extensions;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Models;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories.MetaData
{
    public class StandardMetaDataFactory : IMetaDataFactory
    {
        public Type MetaDataType => typeof(LarsStandard);

        public object Create(IReadOnlyList<string> values)
        {
            if (values == null)
            {
                return null;
            }

            if (values.Count < 11)
            {
                return null;
            }

            var standardid = GetStandardId(values);

            if (standardid < 0)
            {
                return null;
            }

            return new LarsStandard
            {
                Id = standardid,
                Title = values[2].RemoveQuotationMark(),
                NotionalEndLevel = values[4].RemoveQuotationMark().SafeParseInt(),
                StandardUrl = values[8],
                SectorSubjectAreaTier1 = values[9].RemoveQuotationMark().SafeParseDouble(),
                SectorSubjectAreaTier2 = values[10].RemoveQuotationMark().SafeParseDouble(),
            };
        }

        private static int GetStandardId(IReadOnlyList<string> values)
        {
            if (values == null || values.Count < 7)
            {
                return -1;
            }

            return values[0].SafeParseInt();
        }
    }
}
