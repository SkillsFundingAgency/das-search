namespace Sfa.Eds.Das.Web.Services.MappingActions
{
    using System;

    using AutoMapper;

    using Sfa.Eds.Das.Core.Domain.Model;
    using Sfa.Eds.Das.Web.Services.MappingActions.Helpers;
    using Sfa.Eds.Das.Web.ViewModels;

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