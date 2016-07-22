using System;
using System.Collections.Generic;
using System.Linq;
using LARSMetaDataExplorer.Extensions;
using LARSMetaDataExplorer.Models;

namespace LARSMetaDataExplorer.MetaData.MetaDataFactories
{
    public class FrameworkAimMetaDataFactory : IMetaDataFactory
    {
        public Type MetaDataType => typeof(FrameworkAimMetaData);
        public object Create(IReadOnlyList<string> values)
        {
            if (values == null || values.Count() < 7)
            {
                return null;
            }

            var frameworkCode = values[0].RemoveQuotationMark().SafeParseInt();

            if (frameworkCode <= 0)
            {
                return null;
            }

            return new FrameworkAimMetaData
            {
                FworkCode = frameworkCode,
                ProgType = values[1].RemoveQuotationMark().SafeParseInt(),
                PwayCode = values[2].RemoveQuotationMark().SafeParseInt(),
                LearnAimRef = values[3].RemoveQuotationMark(),
                EffectiveFrom = values[4].SafeParseDate() ?? DateTime.MinValue,
                EffectiveTo = values[5].SafeParseDate(),
                FrameworkComponentType = values[6].RemoveQuotationMark().SafeParseInt()
            };
        }
    }
}
