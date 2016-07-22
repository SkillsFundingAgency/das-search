using System;
using System.Collections.Generic;
using LARSMetaDataToolBox.Extensions;
using LARSMetaDataToolBox.Models;

namespace LARSMetaDataToolBox.MetaData.MetaDataFactories
{
    public class FrameworkMetaDataFactory : IMetaDataFactory
    {
        public Type MetaDataType => typeof(FrameworkMetaData);

        public object Create(IReadOnlyList<string> values)
        {
            if (values == null || values.Count <= 11)
            {
                return null;
            }

            var frameworkCode = values[0].RemoveQuotationMark().SafeParseInt();

            if (frameworkCode <= 0)
            {
                return null;
            }

            return new FrameworkMetaData
            {
                FworkCode = frameworkCode,
                ProgType = values[1].RemoveQuotationMark().SafeParseInt(),
                PwayCode = values[2].RemoveQuotationMark().SafeParseInt(),
                PathwayName = values[3].RemoveQuotationMark(),
                EffectiveFrom = values[4].RemoveQuotationMark().SafeParseDate() ?? DateTime.MinValue,
                EffectiveTo = values[5].RemoveQuotationMark().SafeParseDate(),
                SectorSubjectAreaTier1 = values[6].RemoveQuotationMark().SafeParseDouble(),
                SectorSubjectAreaTier2 = values[7].RemoveQuotationMark().SafeParseDouble(),
                NasTitle = values[9].RemoveQuotationMark()
            };
        }
    }
}
