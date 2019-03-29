using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Sfa.Das.Sas.Shared.Components.Domain.Interfaces;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents
{
    public class FatSearchViewModel
    {
        public string Keywords { get; set; }
        public string SearchRouteName { get; set; }
        public string ClassPrefix { get; set; }
        public string ClassModifier { get; set; }
        public ICssClasses CssClasses { get; set; }
    }
}
