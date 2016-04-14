namespace Sfa.Das.WebTest.Infrastructure
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class PageNavigationAttribute : Attribute
    {
        public PageNavigationAttribute(string url)
        {
            this.Url = url;
        }

        public bool IsAbsoluteUrl { get; set; }

        public string Url { get; private set; }

    }
}