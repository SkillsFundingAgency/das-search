﻿using Sfa.Eds.Das.Core.Domain.Model;

namespace Sfa.Eds.Das.Web.ViewModels
{
    public class StandardViewModel
    {
        public Standard Standard { get; set; }

        public bool HasError { get; set; }

        public LinkViewModel SearchResultLink { get; set; }
    }
}