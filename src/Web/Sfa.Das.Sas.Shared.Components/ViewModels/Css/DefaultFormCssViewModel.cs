using System;
using Sfa.Das.Sas.Shared.Components.Domain;
using Sfa.Das.Sas.Shared.Components.ViewModels.Css.Interfaces;

namespace Sfa.Das.Sas.Shared.Components.ViewModels.Css
{
    public class DefaultFormCssViewModel : IDefaultFormCssViewModel
    {
        public DefaultFormCssViewModel(string classPrefix)
        {
            _classPrefix = classPrefix;
        }
        private readonly string _classPrefix;
        public string Input => $"{_classPrefix}input";
        public string FormGroup => $"{_classPrefix}form-group";

        public string Radio => $"{_classPrefix}radio";
        public string RadioInput => $"{_classPrefix}radio__input";
        public string RadioGroupInline => $"{Radio} radio-inline";
        public string Label => $"{_classPrefix}label";
    }
}
