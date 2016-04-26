using System.Collections.Generic;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Models.GovLearn
{
    internal sealed class GovLearnResponse
    {
        public string Description { get; set; }

        public List<GovLearnResource> Resources { get; set; }
    }
}