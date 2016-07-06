using System;
using System.Collections.Generic;
using System.Linq;
using Sfa.Das.Sas.Indexer.Core.Extensions;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories.MetaData
{
    public class FrameworkAimMetaDataFactory : MetaDataFactoryBase, IMetaDataFactory
    {
        public Type MetaDataType => typeof(FrameworkAimMetaData);
        public object Create(IReadOnlyList<string> values)
        {
            if (values == null || values.Count() < 7)
            {
                return null;
            }

            var frameworkCode = TryParse(values[0]);

            if (frameworkCode <= 0)
            {
                return null;
            }

            return new FrameworkAimMetaData
            {
                FworkCode = frameworkCode,
                ProgType = TryParse(values[1].RemoveQuotationMark()),
                PwayCode = TryParse(values[2].RemoveQuotationMark()),
                LearnAimRef = values[3].RemoveQuotationMark(),
                EffectiveFrom = TryGetDate(values[4]) ?? DateTime.MinValue,
                EffectiveTo = TryGetDate(values[5]),
                FrameworkComponentType = TryParse(values[6])
            };
        }
    }
}
