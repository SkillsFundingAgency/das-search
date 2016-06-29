using System;
using FeatureToggle.Core;

namespace Sfa.Das.Sas.ApplicationServices.FeatureToggles
{
    public static class FeatureToggleHelper
    {
        public static bool IsFeatureEnabled<T>()
            where T : IFeatureToggle, new()
        {
            var toggle = new T();

            return toggle.FeatureEnabled;
        }
    }
}
