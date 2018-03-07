using FeatureToggle.Core;
using FeatureToggle.Toggles;

namespace Sfa.Das.Sas.Infrastructure.FeatureToggles
{
    public sealed class IgnoreSslCertificateFeature : SimpleFeatureToggle
    {
        public override IBooleanToggleValueProvider ToggleValueProvider { get => new CloudConfigToggleValueProvider(); set => base.ToggleValueProvider = value; }
    }
}
