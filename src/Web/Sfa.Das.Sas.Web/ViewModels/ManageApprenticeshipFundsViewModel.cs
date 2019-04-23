using System;

namespace Sfa.Das.Sas.Web.ViewModels
{
    public class ManageApprenticeshipFundsViewModel
    {
        private readonly Uri _url;

        public ManageApprenticeshipFundsViewModel(bool isLevyPayer, Uri url)
        {
            IsLevyPayer = isLevyPayer;
            _url = url;
        }

        public string Url => _url.ToString();

        public bool IsLevyPayer
        {
            get;
        }
    }
}