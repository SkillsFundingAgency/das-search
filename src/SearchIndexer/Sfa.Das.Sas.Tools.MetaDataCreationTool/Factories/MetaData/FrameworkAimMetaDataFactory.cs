using System;
using System.Collections.Generic;
using System.Linq;
using Sfa.Das.Sas.Indexer.Core.Extensions;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories.MetaData
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
