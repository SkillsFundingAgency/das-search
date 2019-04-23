using Sfa.Das.Sas.Shared.Components.Domain.Interfaces;

namespace Sfa.Das.Sas.Shared.Components.Domain
{
    public class DefaultUtilitiesCssClasses : IUtilitiesCssClasses
    {
        public DefaultUtilitiesCssClasses()
        {

        }
        public DefaultUtilitiesCssClasses(string classPrefix)
        {
            ClassPrefix = classPrefix;
        }

        public string ClassPrefix { get; set; } = "govuk-";
        public string FontWeightBold => $"{ClassPrefix}!-font-weight-bold";
        public string Margin(string type, int size)
        {
            return $"{ClassPrefix}!-margin-{type}-{size}";
        }
    }
}
