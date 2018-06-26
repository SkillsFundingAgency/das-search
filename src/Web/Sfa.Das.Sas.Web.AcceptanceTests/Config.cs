﻿using Sfa.Automation.Framework.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
