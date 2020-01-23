using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Shared.Components.Web
{
    public class LayoutService : ILayoutService
    {
        public string Layout { get; set; } = "_Layout";
        public string CssPrefix()
        {
            return Layout == "_Layout" ? "govuk-" : string.Empty;
        }

        public string CssModifier()
        {
            return Layout == "_Layout" ? string.Empty : "employer";
        }

        public LayoutType LayoutType()
        {
            return Layout == "_Layout" ? Web.LayoutType.Gds : Web.LayoutType.Campaign;
        }

        public string Host { get; set; } = "https://das-prd-frnt-end.azureedge.net";
    }

    public interface ILayoutService
    {
        string Layout { get; set; }

        string CssPrefix();
        string CssModifier();
        LayoutType LayoutType();
        string Host { get; set; }

    }

    public enum LayoutType
    {
        Gds = 0,
        Campaign = 1
    }
}
