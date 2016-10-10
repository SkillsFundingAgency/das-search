using System;
using System.Linq;
using System.Reflection;

namespace PactNet.TestExtensions
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PactProviderAttribute : Attribute
    {
        public readonly string ProviderName;

        public PactProviderAttribute(string providerName)
        {
            ProviderName = providerName;
        }

        public static string GetProviderName(object source)
        {
            var info = source.GetType();
            var attributes = info.GetCustomAttributes(true).OfType<PactProviderAttribute>().ToList();
            if (!attributes.Any())
            {
                throw new CustomAttributeFormatException("Missing the PactProviderAttribute to specify the name of the provider");
            }

            return attributes.FirstOrDefault()?.ProviderName;
        }
    }
}