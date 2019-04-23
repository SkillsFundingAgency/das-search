using System;
using System.Collections.Generic;
using System.Text;

namespace Sfa.Das.Sas.Shared.Components.Domain.Interfaces
{
    public interface ICssClasses
    {
        string ClassModifier { get; set; }
        string ClassPrefix { get; set; }
        string ButtonCss { get; }
        string InputCss { get; }
        string FormGroupCss { get; }

    }
}
