﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Das.Sas.Shared.Components.Domain;
using Sfa.Das.Sas.Shared.Components.ViewModels.Css;
using Sfa.Das.Sas.Shared.Components.ViewModels.Css.Interfaces;

namespace Sfa.Das.Sas.Shared.Components.Web.Models
{
    public class CssSwitcherViewModel : ICssViewModel
    {
        private readonly ILayoutService _layoutService;
        public CssSwitcherViewModel(ILayoutService layoutService)
        {
            _layoutService = layoutService;
            ClassPrefix = _layoutService.CssPrefix();
            ClassModifier = _layoutService.CssModifier();
        }

        public ITableCssViewModel Table => new DefaultTableCssViewModel(ClassPrefix);
        public IUtilitiesCssViewModel UtilitiesCss => new DefaultUtilitiesCssViewModel();
        public IDefaultFormCssViewModel FormCss => new DefaultFormCssViewModel(ClassPrefix);
        public ICssGridViewModel GridCss => new DefaultGridCssViewModel(ClassPrefix);
        public IErrorCssViewModel ErrorCss => new DefaultErrorCssViewModel(ClassPrefix);
        public string ClassModifier { get; set; } = string.Empty;
        public string ClassPrefix { get; set; }
        private string _buttonCss => $"{ClassPrefix}button";

        public string Button
        {
            get
            {
                if (_layoutService.LayoutType() == LayoutType.Campaign)
                {
                    if (String.IsNullOrWhiteSpace(ClassModifier))
                    {
                        return $"{_buttonCss} button--sparks";
                    }
                    else
                    {
                        return $"{_buttonCss} button--sparks button-{ClassModifier}";
                    }
                }


                if (String.IsNullOrWhiteSpace(ClassModifier))
                {
                    return $"{ClassPrefix}button";
                }
                else
                {
                    return $"{ClassPrefix}button {ClassPrefix}button--{ClassModifier}";
                }
            }
        }
        public string ButtonSecondary
        {
            get
            {
                if (_layoutService.LayoutType() == LayoutType.Campaign)
                {
                    if (String.IsNullOrWhiteSpace(ClassModifier))
                    {
                        return _buttonCss;
                    }
                    else
                    {
                        return $"{_buttonCss} button-inverted";
                    }
                }

                if (String.IsNullOrWhiteSpace(ClassModifier))
                {
                    return $"{ClassPrefix}button";
                }
                else
                {
                    return $"{ClassPrefix}button {ClassPrefix}button--{ClassModifier}";
                }
            }
        }
        public string Link => $"{ClassPrefix}link";

        public string List
        {
            get
            {
                if (String.IsNullOrWhiteSpace(ClassModifier))
                {
                    return $"{ClassPrefix}list";
                }
                else
                {
                    return $"{ClassPrefix}list {ClassPrefix}list--{ClassModifier}";
                }
            }
        }

        public string ListBullet => $"{ClassPrefix}list--bullet";

        public string ListNumber => $"{ClassPrefix}list--number";
        public string ListNumbers => $"{ClassPrefix}list-numbers";
        public string SearchList
        {
            get
            {
                if (String.IsNullOrWhiteSpace(ClassModifier))
                {
                    return $"{List} {ListNumber} das-search-results__list";
                }
                else
                {
                    return $"{List} {ListNumber} das-search-results__list das-search-results__list--{ClassModifier}";
                }
            }
        }

        public string WarningText => $"{ClassPrefix}warning-text";
        private string _heading => $"{ClassPrefix}heading";
        public string HeadingMedium => $"{_heading}-m";
        public string HeadingLarge => $"{_heading}-l";
        public string HeadingXLarge => $"{_heading}-xl";
        public string HeadingSmall => $"{_heading}-s";
        public string HeadingXSmall => $"{_heading}-xs";
        public string Details => $"{ClassPrefix}details";
        public string DetailsSummary => $"{Details}__summary";
        public string DetailsSummaryText => $"{Details}__summary-text";
        public string DetailsText => $"{Details}__text";

        public string VisuallyHidden => $"{ClassPrefix}visually-hidden";
    }
}
