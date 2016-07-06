using System;
using System.Collections.Generic;
using Sfa.Das.Sas.Indexer.Core.Extensions;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories.MetaData
{
    public class FrameworkMetaDataFactory : MetaDataFactoryBase, IMetaDataFactory
    {
        public Type MetaDataType => typeof(FrameworkMetaData);

        public object Create(IReadOnlyList<string> values)
        {
            if (values == null || values.Count <= 11)
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
    }
}
