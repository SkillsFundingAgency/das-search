using System;
using System.Collections.Generic;
using Sfa.Das.Sas.Indexer.Core.Extensions;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories.MetaData
{
    public class FundingMetaDataFactory : IMetaDataFactory
    {
        public Type MetaDataType => typeof(FundingMetaData);
        public object Create(IReadOnlyList<string> values)
        {
            if (values == null || values.Count <= 5)
            {
                return null;
            }

            var rate = Convert.ToInt32(values[4].SafeParseDouble());

            Console.WriteLine($"Funding rate for {values[0].RemoveQuotationMark()}: {rate}");

            return new FundingMetaData
            {
                LearnAimRef = values[0].RemoveQuotationMark(),
                FundingCategory = values[1],
                EffectiveFrom = values[2].SafeParseDate(),
                EffectiveTo = values[3].SafeParseDate(),
                RateWeighted = rate
            };
        }
    }
}
