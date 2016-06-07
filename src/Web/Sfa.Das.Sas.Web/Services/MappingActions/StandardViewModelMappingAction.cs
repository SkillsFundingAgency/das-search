using System;
using AutoMapper;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Web.Services.MappingActions.Helpers;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Services.MappingActions
{

    public class StandardViewModelMappingAction : IMappingAction<Standard, StandardViewModel>
    {
        public void Process(Standard source, StandardViewModel destination)
        {
            destination.TypicalLengthMessage = StandardMappingHelper.GetTypicalLengthMessage(source.TypicalLength);

            destination.StandardPdfUrlTitle = ExtractPdfTitle(source.StandardPdf);
            destination.AssessmentPlanPdfUrlTitle = ExtractPdfTitle(source.AssessmentPlanPdf);
        }

        private string ExtractPdfTitle(string pdfUrl)
        {
            if (string.IsNullOrEmpty(pdfUrl))
            {
                return string.Empty;
            }

            var lastIndex = pdfUrl.LastIndexOf("/", StringComparison.Ordinal) + 1;
            var pdfName = pdfUrl;

            if (lastIndex >= 0)
            {
                 pdfName = pdfUrl.Substring(lastIndex);
            }

            return pdfName.Replace("_", " ").Replace(".pdf", string.Empty);
        }
    }
}