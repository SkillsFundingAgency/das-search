using System;
using Sfa.Das.Sas.Shared.Components.Domain.Interfaces;

namespace Sfa.Das.Sas.Shared.Components.Domain
{
    public class DefaultTableCssClasses : ITableCssClasses
    {
        public DefaultTableCssClasses()
        {

        }
        public DefaultTableCssClasses(string classPrefix)
        {
            _classPrefix = classPrefix;
        }

        public string _classPrefix { get; }
        public string Table => $"{_classPrefix}table";
        public string Head => $"{_classPrefix}table__head";
        public string Header => $"{_classPrefix}table__header";
        public string Row => $"{_classPrefix}table__row";
        public string Body => $"{_classPrefix}table__body";
        public string Cell => $"{_classPrefix}table__cell";
    }
}
