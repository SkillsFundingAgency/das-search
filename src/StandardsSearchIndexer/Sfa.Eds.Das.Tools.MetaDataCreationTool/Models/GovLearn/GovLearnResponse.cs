namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Models.GovLearn
{
    using System.Collections.Generic;

    internal sealed class GovLearnResponse
    {
        public string Description { get; set; }

        public List<GovLearnResource> Resources { get; set; }
    }
}