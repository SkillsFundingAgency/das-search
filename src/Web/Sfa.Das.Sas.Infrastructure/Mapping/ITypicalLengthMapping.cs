using ApiTypicalLength = SFA.DAS.Apprenticeships.Api.Client.Models.TypicalLength;
using DomainTypicalLength = Sfa.Das.Sas.Core.Domain.Model.TypicalLength;

namespace Sfa.Das.Sas.Infrastructure.Mapping
{
    public interface ITypicalLengthMapping
    {
        DomainTypicalLength MapTypicalLength(ApiTypicalLength typicalLength);
    }
}