using MediatR;

namespace Sfa.Das.Sas.ApplicationServices.Queries
{
    public class GetStandardQuery : IRequest<GetStandardResponse>
    {
        public int Id { get; set; }

        public string Keywords { get; set; }
    }
}
