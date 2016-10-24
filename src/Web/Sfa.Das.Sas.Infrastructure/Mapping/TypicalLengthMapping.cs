using ApiTypicalLength = SFA.DAS.Apprenticeships.Api.Types.TypicalLength;
using DomainTypicalLength = Sfa.Das.Sas.Core.Domain.Model.TypicalLength;

namespace Sfa.Das.Sas.Infrastructure.Mapping
{
    public class TypicalLengthMapping : ITypicalLengthMapping
    {
        public DomainTypicalLength MapTypicalLength(ApiTypicalLength typicalLength)
        {
            return new DomainTypicalLength
            {
                From = typicalLength.From,
                To = typicalLength.To,
                Unit = typicalLength.Unit
            };
        }
    }
}