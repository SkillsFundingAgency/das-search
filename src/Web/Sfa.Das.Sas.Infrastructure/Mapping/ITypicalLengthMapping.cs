using ApiTypicalLength = SFA.DAS.Apprenticeships.Api.Types;
using DomainTypicalLength = Sfa.Das.Sas.Core.Domain.Model.TypicalLength;

namespace Sfa.Das.Sas.Infrastructure.Mapping
{
    public interface ITypicalLengthMapping
    {
        DomainTypicalLength MapTypicalLength(ApiTypicalLength.TypicalLength typicalLength);
    }
}