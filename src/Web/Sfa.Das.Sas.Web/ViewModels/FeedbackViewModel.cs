using System;
using SFA.DAS.Apprenticeships.Api.Types.Providers;

namespace Sfa.Das.Sas.Web.ViewModels
{
    public class FeedbackViewModel : Feedback
    {
        public FeedbackViewModel()
        {
        }

        public FeedbackViewModel(Feedback providerFeedback)
        {
            ExcellentFeedbackCount = providerFeedback.ExcellentFeedbackCount;
            GoodFeedbackCount = providerFeedback.GoodFeedbackCount;
            PoorFeedbackCount = providerFeedback.PoorFeedbackCount;
            VeryPoorFeedbackCount = providerFeedback.VeryPoorFeedbackCount;
            Strengths = providerFeedback.Strengths;
            Weaknesses = providerFeedback.Weaknesses;
        }

        public int TotalFeedbackCount => ExcellentFeedbackCount + GoodFeedbackCount + PoorFeedbackCount + VeryPoorFeedbackCount;
        public decimal ExcellentFeedbackPercentage => CalculatePercentageOfTotal(ExcellentFeedbackCount);
        public decimal GoodFeedbackPercentage => CalculatePercentageOfTotal(GoodFeedbackCount);
        public decimal PoorFeedbackPercentage => CalculatePercentageOfTotal(PoorFeedbackCount);
        public decimal VeryPoorFeedbackPercentage => CalculatePercentageOfTotal(VeryPoorFeedbackCount);

        private decimal CalculatePercentageOfTotal(int feedbackCount)
        {
            return Math.Round((decimal)(feedbackCount * 100) / TotalFeedbackCount, 2);
        }
    }
}