namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Views.Provider
{
    using System.Collections.Generic;
    using ExtensionHelpers;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;
    using Sas.Web.Views.Provider;
    using SFA.DAS.Apprenticeships.Api.Types;
    using SFA.DAS.Apprenticeships.Api.Types.Pagination;
    using SFA.DAS.Apprenticeships.Api.Types.Providers;
    using ViewModels;

    [TestFixture]
    public class ProviderDetailPageTests : ViewTestBase
    {
        internal const string TradingNames = "item 1, Another Item, A different item";
        internal const string Phone = "123-456";
        internal const string ProviderName = "Joe The Plumbers";
        internal const string Email = "test@test.com";
        internal const long UkPrn = 2221221;
        internal const string Website = "http://test.com";
        internal const double NoSatisfactionScore = 0;
        internal const string NoSatisfactionScoreMessage = "no data available";
        internal const double SatisfactionScore = 15.9;
        internal const string SatisfactionScoreMessage = "15.9%";
        internal const string ParentCompanyGuaranteeMessage = "Provider is supported by a parent company guarantee";
        internal const string IsNewProviderMessage = "New organisation with no financial track record";
        internal const string IsLevyPayerOnlyMessage = "Only levy paying employers can work with this provider";

        [Test]
        public void ShouldShowFieldsAsExpected()
        {
            var providerDetails = new ProviderDetail();
            var model = GetProvider();

            var html = providerDetails.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "#provider-name").Should().Contain(ProviderName);
            GetPartial(html, ".tag-national").Should().Contain("National");
            GetPartial(html, "#trading-names").Should().Contain(TradingNames);
            GetPartial(html, ".data-list--provider").Should().Contain(UkPrn.ToString());

            GetPartial(html, ".apprenticeshipContact").Should().Contain(ProviderName + " website");
            GetAttribute(html, ".apprenticeshipContact", "href").Should().Be(Website, "because http be added if missing");
            GetPartial(html, ".phone").Should().Contain(model.Phone);
            GetPartial(html, ".email").Should().Contain(model.Email);
            GetPartial(html, "#employer-satisfaction").Should().Be(NoSatisfactionScoreMessage);
            GetPartial(html, "#learner-satisfaction").Should().Contain(SatisfactionScoreMessage);
            GetPartial(html, ".satisfaction-source").Should().Contain("Source:");
        }

        [Test]
        public void ShouldShowNoNationalFieldIfNationalIsFalse()
        {
            var providerDetails = new ProviderDetail();
            var model = GetProvider();
            model.NationalProvider = false;
            var html = providerDetails.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, ".tag-national").Should().Be(string.Empty);
        }

        [Test]
        public void ShouldShowNoTradingNamesFieldIfNoTradingNames()
        {
            var providerDetails = new ProviderDetail();
            var model = GetProvider();
            model.TradingNames = string.Empty;
            var html = providerDetails.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "#trading-names").Should().Be(string.Empty);
        }

        [Test]
        public void ShouldShowEmployerSatisfactionIfNot0()
        {
            var providerDetails = new ProviderDetail();
            var model = GetProvider();
            model.EmployerSatisfaction = SatisfactionScore;
            model.EmployerSatisfactionMessage = SatisfactionScoreMessage;
            var html = providerDetails.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "#employer-satisfaction").Should().Contain(SatisfactionScoreMessage);
        }

        [Test]
        public void ShouldShowLearnerSatisfactionIf0()
        {
            var providerDetails = new ProviderDetail();
            var model = GetProvider();
            model.LearnerSatisfaction = NoSatisfactionScore;
            model.LearnerSatisfactionMessage = NoSatisfactionScoreMessage;
            var html = providerDetails.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "#learner-satisfaction").Should().Be(NoSatisfactionScoreMessage);
        }

        [Test]
        public void ShouldNotShowSourceIfNoSatisfactionScores()
        {
            var providerDetails = new ProviderDetail();
            var model = GetProvider();
            model.LearnerSatisfaction = NoSatisfactionScore;
            model.LearnerSatisfactionMessage = NoSatisfactionScoreMessage;

            var html = providerDetails.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, ".satisfaction-source").Should().Be(string.Empty);
        }

        [Test]
        public void ShouldShowParentCompanyGuaranteeMessageIfSet()
        {
            var providerDetails = new ProviderDetail();
            var model = GetProvider();
            model.HasParentCompanyGuarantee = true;
            model.IsLevyPayerOnly = false;
            model.IsNew = false;

            var html = providerDetails.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "#levy-payer-only").Should().Be(string.Empty);
            GetPartial(html, "#parent-company-guarantee").Should().Contain(ParentCompanyGuaranteeMessage);
            GetPartial(html, "#is-new-provider").Should().Be(string.Empty);
        }

        [Test]
        public void ShouldShowIsLevyNewMessageIfSet()
        {
            var providerDetails = new ProviderDetail();
            var model = GetProvider();
            model.HasParentCompanyGuarantee = false;
            model.IsLevyPayerOnly = false;
            model.IsNew = true;

            var html = providerDetails.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "#levy-payer-only").Should().Be(string.Empty);
            GetPartial(html, "#parent-company-guarantee").Should().Be(string.Empty);
            GetPartial(html, "#is-new-provider").Should().Contain(IsNewProviderMessage);
        }

        [Test]
        public void ShouldShowAboutThisProviderIfNoTradingNamesIsSet()
        {
            var providerDetails = new ProviderDetail();
            var model = GetProvider();
            model.TradingNames = string.Empty;
            model.HasMoreThanOneTradingName = false;

            var html = providerDetails.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "#about-this-provider").Should().Contain("About this Provider");
        }

        [Test]
        public void ShouldNotShowAboutThisProviderIfOneAndOnlyOneTradingNamesIsSet()
        {
            var providerDetails = new ProviderDetail();
            var model = GetProvider();
            model.TradingNames = "here is a trading name";
            model.HasMoreThanOneTradingName = false;

            var html = providerDetails.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "#about-this-provider").Should().Contain("About this Provider");
        }

        [Test]
        public void ShouldNotShowAboutThisProviderIfMoreThanOneTradingNamesIsSet()
        {
            var providerDetails = new ProviderDetail();
            var model = GetProvider();
            model.TradingNames = "here is a trading name, here is another tradingname";
            model.HasMoreThanOneTradingName = true;

            var html = providerDetails.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "#about-this-provider").Should().Be(string.Empty);
        }

        [Test]
        public void ShouldShowIsLevyPayerOnlyMessageIfSet()
        {
            var providerDetails = new ProviderDetail();
            var model = GetProvider();
            model.HasParentCompanyGuarantee = false;
            model.IsLevyPayerOnly = true;
            model.IsNew = false;

            var html = providerDetails.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "#levy-payer-only").Should().Contain(IsLevyPayerOnlyMessage);
            GetPartial(html, "#parent-company-guarantee").Should().Be(string.Empty);
            GetPartial(html, "#is-new-provider").Should().Be(string.Empty);
        }

        [Test]
        public void ShouldNotShowAnyNavigationDetailsIfOnlyOnePage()
        {
            var providerDetails = new ProviderDetail();
            var model = GetProvider();
            var html = providerDetails.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "#previous-nav").Should().Be(string.Empty);
            GetPartial(html, "#next-nav").Should().Be(string.Empty);
        }

        [Test]
        public void ShoulShowOnlyNextNavigationDetailsIfMoreThanOnePageAndOnFirstPage()
        {
            var providerDetails = new ProviderDetail();
            var model = GetProvider();
            model.ApprenticeshipTrainingSummary.PaginationDetails
                = new PaginationDetails
                {
                        LastPage = 2,
                        NumberOfRecordsToSkip = 0,
                        NumberPerPage = 20,
                        TotalCount = 1,
                        Page = 1
                };

            var html = providerDetails.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "#previous-nav").Should().Be(string.Empty);
            GetPartial(html, "#next-nav").Should().Contain("2 of 2");
        }

        [Test]
        public void ShoulShowBothNavigationDetailsIfSecondPageOfFour()
        {
            var providerDetails = new ProviderDetail();
            var model = GetProvider();
            model.ApprenticeshipTrainingSummary.PaginationDetails
                = new PaginationDetails
                {
                    LastPage = 4,
                    NumberOfRecordsToSkip = 20,
                    NumberPerPage = 20,
                    TotalCount = 69,
                    Page = 2
                };

            var html = providerDetails.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "#previous-nav").Should().Contain("1 of 4");
            GetPartial(html, "#next-nav").Should().Contain("3 of 4");
        }

        [Test]
        public void ShoulShowOnlyPreviousNavigationDetailsIfLastPage()
        {
            var providerDetails = new ProviderDetail();
            var model = GetProvider();
            model.ApprenticeshipTrainingSummary.PaginationDetails
                = new PaginationDetails
                {
                    LastPage = 4,
                    NumberOfRecordsToSkip = 60,
                    NumberPerPage = 20,
                    TotalCount = 69,
                    Page = 4
                };

            var html = providerDetails.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "#previous-nav").Should().Contain("3 of 4");
            GetPartial(html, "#next-nav").Should().Be(string.Empty);
        }

        private static ProviderDetailViewModel GetProvider()
        {
            return new ProviderDetailViewModel
            {
                TradingNames = TradingNames,
                EmployerSatisfaction = NoSatisfactionScore,
                EmployerSatisfactionMessage = NoSatisfactionScoreMessage,
                LearnerSatisfaction = SatisfactionScore,
                LearnerSatisfactionMessage = SatisfactionScoreMessage,
                Email = Email,
                IsEmployerProvider = true,
                IsHigherEducationInstitute = true,
                NationalProvider = true,
                Phone = Phone,
                UkPrn = UkPrn,
                ProviderName = ProviderName,
                Website = Website,
                ApprenticeshipTrainingSummary = new ApprenticeshipTrainingSummary
                {
                    ApprenticeshipTrainingItems = new List<ApprenticeshipTraining> {
                        new ApprenticeshipTraining
                        {
                            Identifier = "1",
                            Name = "Aerospace",
                            Level = 1,
                            TrainingType = ApprenticeshipTrainingType.Framework,
                            Type = "Framework"
                        }
                    },
                    PaginationDetails = new PaginationDetails { LastPage = 1, NumberOfRecordsToSkip = 0, NumberPerPage = 20, TotalCount = 1, Page = 1 }
                }
            };
        }
    }
}
