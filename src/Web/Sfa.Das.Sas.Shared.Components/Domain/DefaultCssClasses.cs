using System;
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
                    return $"{ClassPrefix}button {ClassPrefix}button--{ClassModifier}";
                }
            }
        }

        public string Input => $"{ClassPrefix}input";
        public string FormGroup => $"{ClassPrefix}form-group";
        public string Link => $"{ClassPrefix}link";

        public string List
        {
            get
            {
                if (String.IsNullOrWhiteSpace(ClassModifier))
                {
                    return $"{ClassPrefix}list";
                }
                else
                {
                    return $"{ClassPrefix}list {ClassPrefix}list--{ClassModifier}";
                }
            }
        }

        public string ListNumber => $"{ClassPrefix}list--number";
        public string SearchList
        {
            get
            {
                if (String.IsNullOrWhiteSpace(ClassModifier))
                {
                    return $"{List} {ListNumber} das-search-results__list";
                }
                else
                {
                    return $"{List} {ListNumber} das-search-results__list das-search-results__list--{ClassModifier}";
                }
            }
        }

        public string WarningText => $"{ClassPrefix}warning-text";
        private string _heading => $"{ClassPrefix}heading";
        public string HeadingMedium => $"{_heading}-m";
        public string HeadingLarge => $"{_heading}-l";
        public string HeadingXLarge => $"{_heading}-xl";
        public string HeadingSmall => $"{_heading}-s";
        public string HeadingXSmall => $"{_heading}-xs";
    }
}
