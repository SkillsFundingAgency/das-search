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

        public string Radio => $"{_classPrefix}radios";
        public string RadioInput => $"{_classPrefix}radios__input";
        public string RadioLabel => $"{_classPrefix}radios__label";
        public string RadioItem => $"{_classPrefix}radios__item";
        public string RadioGroupInline => $"{Radio} radios--inline";
        public string Label => $"{_classPrefix}label";

        public string Checkbox => $"{_classPrefix}checkbox";
        public string CheckboxInput => $"{_classPrefix}checkboxes__input";
        public string CheckboxItem => $"{_classPrefix}checkboxes__item";
        public string CheckboxLabel => $"{Label} {_classPrefix}checkboxes__label";
    }
}
