using System;
using System.Collections.Generic;
using LARSMetaDataExplorer.Extensions;
using LARSMetaDataExplorer.Models;

namespace LARSMetaDataExplorer.MetaData.MetaDataFactories
{
    public class LearningDeliveryMetaDataFactory : IMetaDataFactory
    {
        public Type MetaDataType => typeof(LearningDeliveryMetaData);

        public object Create(IReadOnlyList<string> values)
        {
            if (values == null || values.Count < 5)
            {
                return null;
            }

            return new LearningDeliveryMetaData
            {
                LearnAimRef = values[0].RemoveQuotationMark(),
                EffectiveFrom = values[1].SafeParseDate() ?? DateTime.MinValue,
                EffectiveTo = values[2].SafeParseDate(),
                LearnAimRefTitle = values[3].RemoveQuotationMark(),
                LearnAimRefType = values[4].RemoveQuotationMark().SafeParseInt()
            };
        }
    }
}
