using System;
using System.Collections.Generic;
using LARSMetaDataToolBox.Extensions;
using LARSMetaDataToolBox.Models;

namespace LARSMetaDataToolBox.MetaData.MetaDataFactories
{
    public class FrameworkComponentTypeMetaDataFactory : IMetaDataFactory
    {
        public Type MetaDataType => typeof(FrameworkComponentTypeMetaData);

        public object Create(IReadOnlyList<string> values)
        {
            if (values == null || values.Count < 5)
            {
                return null;
            }

            return new FrameworkComponentTypeMetaData
            {
                FrameworkComponentType = values[0].RemoveQuotationMark().SafeParseInt(),
                FrameworkComponentTypeDesc = values[1].RemoveQuotationMark(),
                FrameworkComponentTypeDesc2 = values[2].RemoveQuotationMark(),
                EffectiveFrom = values[3].SafeParseDate() ?? DateTime.MinValue,
                EffectiveTo = values[4].SafeParseDate()
            };
        }
    }
}
