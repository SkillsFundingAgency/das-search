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
        public string ClassModifier { get; set; } = string.Empty;
        public string ClassPrefix { get; set; } = "govuk-";

        public string ButtonCss
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

        public string InputCss => $"{ClassPrefix}input";
        public string FormGroupCss => $"{ClassPrefix}form-group";
    }
}
