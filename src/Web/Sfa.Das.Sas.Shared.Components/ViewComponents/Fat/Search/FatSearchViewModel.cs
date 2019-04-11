using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Sfa.Das.Sas.Shared.Components.Domain.Interfaces;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents
{
    public class FatSearchViewModel : SearchQueryViewModel
    {
        public string FatSearchRoute { get; set; } = "/Fat/Search";
    }
}
