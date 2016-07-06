using System;
using System.Collections.Generic;
using Sfa.Das.Sas.Indexer.Core.Extensions;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories.MetaData
{
    public class LearningDeliveryMetaDataFactory : MetaDataFactoryBase, IMetaDataFactory
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
                EffectiveFrom = TryGetDate(values[1]) ?? DateTime.MinValue,
                EffectiveTo = TryGetDate(values[2]),
                LearnAimRefTitle = values[3].RemoveQuotationMark(),
                LearnAimRefType = TryParse(values[4].RemoveQuotationMark())
            };
        }
    }
}
