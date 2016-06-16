namespace Sfa.Das.Sas.Web.ViewModels
{
    using System.Collections.Generic;

    public interface IShortlistApprenticeshipViewModel
    {
        int Id { get; set; }
        ApprenticeshipTrainingType TrainingType { get; }
        string Title { get; set; }
        int Level { get; set; }
        List<ShortlistProviderViewModel> Providers { get; set; }
    }
}