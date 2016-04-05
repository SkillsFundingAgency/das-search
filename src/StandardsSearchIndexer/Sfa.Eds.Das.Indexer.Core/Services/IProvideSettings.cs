using System;
namespace Sfa.Eds.Das.Indexer.Core.Services
{
    public interface IProvideSettings
    {
        string GetSetting(string settingKey);
    }
}
