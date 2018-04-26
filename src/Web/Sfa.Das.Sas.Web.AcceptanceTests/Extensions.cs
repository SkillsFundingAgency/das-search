using System;
using System.IO;

namespace Sfa.Das.Sas.Web.AcceptanceTests
{
    public static class Extensions
    {
        public static Uri Combine(this Uri uri, string part)
        {
            return new Uri(Path.Combine(uri.ToString(), part).Replace("\\", "/"));
        }
    }
}
