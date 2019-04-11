using System;
using System.Collections.Generic;
using System.Text;
using Sfa.Das.Sas.Shared.Components.Domain.Interfaces;

namespace Sfa.Das.Sas.Shared.Components.Domain
{
    public class DefaultCssClasses : ICssClasses
    {
        public DefaultCssClasses()
        {

        }
        public DefaultCssClasses(string classPrefix)
        {
            ClassPrefix = classPrefix;
        }

        public IUtilitiesCssClasses UtilitiesCss => new DefaultUtilitiesCssClasses();
        public string ClassModifier { get; set; } = string.Empty;
        public string ClassPrefix { get; set; } = "govuk-";

        public string Button
        {
            get
            {
                if (String.IsNullOrWhiteSpace(ClassModifier))
                {
                    return $"{ClassPrefix}button";
                }
                else
                {
                    return $"{ClassPrefix}button button-{ClassModifier}";
                }
            }
        }

        public string Input => $"{ClassPrefix}input";
        public string FormGroup => $"{ClassPrefix}form-group";
        public string Link => $"{ClassPrefix}link";
        public string List => $"{ClassPrefix}list";
        public string WarningText => $"{ClassPrefix}warning-text";
        private string _heading => $"{ClassPrefix}heading";
        public string HeadingMedium => $"{_heading}-m";
        public string HeadingLarge => $"{_heading}-l";
        public string HeadingXLarge => $"{_heading}-xl";
        public string HeadingSmall => $"{_heading}-s";
        public string HeadingXSmall => $"{_heading}-xs";
    }
}
