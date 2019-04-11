using System;
using System.Collections.Generic;
using System.Text;

namespace Sfa.Das.Sas.Shared.Components.Domain.Interfaces
{
    public interface ICssClasses
    {
        IUtilitiesCssClasses UtilitiesCss { get; }
        string ClassModifier { get; set; }
        string ClassPrefix { get; }
        string Button { get; }
        string Input { get; }
        string FormGroup { get; }
        string Link { get; }
        string List { get; }
        
        string HeadingMedium { get; }
        string HeadingLarge { get; }
        string HeadingXLarge { get; }
        string HeadingSmall { get; }
        string HeadingXSmall { get; }

    }
}
