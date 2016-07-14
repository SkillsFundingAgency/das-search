﻿using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.ApplicationServices.Queries
{
    public class GetFrameworkResponse
    {
        public enum ResponseCodes
        {
            Success,
            InvalidFrameworkId,
            FrameworkNotFound
        }

        public ResponseCodes StatusCode { get; set; }

        public Framework Framework { get; set; }

        public bool IsShortlisted { get; set; }

        public string SearchTerms { get; set; }
    }
}
