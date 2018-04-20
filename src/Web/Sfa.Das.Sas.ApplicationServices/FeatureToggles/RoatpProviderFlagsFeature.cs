using FeatureToggle.Core;
using FeatureToggle.Toggles;

namespace Sfa.Das.Sas.ApplicationServices.FeatureToggles
{
    public class RoatpProviderFlagsFeature : SimpleFeatureToggle
    {
        public override IBooleanToggleValueProvider ToggleValueProvider
        {
            get { return new CloudConfigToggleValueProvider(); }
            set { base.ToggleValueProvider = value; }
        }
    }
}
