using SFA.DAS.Apprenticeships.Api.Types.Providers;

namespace Sfa.Das.Sas.Web.ViewModels
{
    public class FeedbackViewModel : Feedback
    {

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
    }
}