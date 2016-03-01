namespace Sfa.Eds.Das.Web.Services.MappingActions
{
    using System;

    using AutoMapper;

    using Sfa.Eds.Das.Core.Domain.Model;
    using Sfa.Eds.Das.Web.Services.MappingActions.Helpers;
    using Sfa.Eds.Das.Web.ViewModels;

    internal class StandardViewModelMappingAction : IMappingAction<Standard, StandardViewModel>
    {
        public void Process(Standard source, StandardViewModel destination)
        {
            destination.TypicalLengthMessage = StandardMappingHelper.GetTypicalLengthMessage(source.TypicalLength);
            destination.IntroductoryTextHtml = MarkdownToHtml(source.IntroductoryText);
            destination.EntryRequirementsHtml = MarkdownToHtml(source.EntryRequirements);
            destination.WhatApprenticesWillLearnHtml = MarkdownToHtml(source.WhatApprenticesWillLearn);
            destination.QualificationsHtml = MarkdownToHtml(source.Qualifications);
            destination.ProfessionalRegistrationHtml = MarkdownToHtml(source.ProfessionalRegistration);
            destination.OverviewOfRoleHtml = MarkdownToHtml(source.OverviewOfRole);

            destination.StandardPdfUrlTitle = ExtractPdfTitle(source.StandardPdfUrl);
            destination.AssessmentPlanPdfUrlTitle = ExtractPdfTitle(source.AssessmentPlanPdfUrl);
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

        private string MarkdownToHtml(string markdownText)
        {
            if (!string.IsNullOrEmpty(markdownText))
            {
                return CommonMark.CommonMarkConverter.Convert(markdownText);
            }

            return string.Empty;
        }
    }
}