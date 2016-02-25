using Sfa.Eds.Das.Core.Domain.Model;

namespace Sfa.Das.ApplicationServices.Models
{
    using System.Collections.Generic;

    public sealed class StandardDetail
    {
        public Standard Standard { get; set; }

        public bool HasError { get; set; }
    }
}