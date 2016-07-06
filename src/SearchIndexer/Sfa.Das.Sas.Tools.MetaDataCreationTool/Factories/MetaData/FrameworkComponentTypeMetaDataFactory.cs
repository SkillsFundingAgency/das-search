using System;
using System.Collections.Generic;
using Sfa.Das.Sas.Indexer.Core.Extensions;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories.MetaData
{
    public class FrameworkComponentTypeMetaDataFactory : MetaDataFactoryBase, IMetaDataFactory
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
                FrameworkComponentType = TryParse(values[0]),
                FrameworkComponentTypeDesc = values[1].RemoveQuotationMark(),
                FrameworkComponentTypeDesc2 = values[2].RemoveQuotationMark(),
                EffectiveFrom = TryGetDate(values[3]) ?? DateTime.MinValue,
                EffectiveTo = TryGetDate(values[4])
            };
        }
    }
}
