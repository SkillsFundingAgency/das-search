using Sfa.Automation.Framework.Enums;
using System;
using System.Configuration;

namespace Sfa.Das.Sas.Web.AcceptanceTests
{
    public class Config
    {
        public string WebSiteUrl => GetAppSetting("WebSiteUrl");

        public WebDriver Browser => Enum.TryParse(GetAppSetting("Browser"), true, out WebDriver browser)
            ? browser
            : throw new InvalidOperationException($"Invalid browser: {GetAppSetting("Browser")}");

        protected string GetAppSetting(string keyName) 
            => ConfigurationManager.AppSettings[keyName] 
            ?? throw new InvalidOperationException($"{keyName} not found in app settings.");
    }
}
