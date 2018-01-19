using FluentAssertions;
using NUnit.Framework;
using RazorGenerator.Testing;
using Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.ExtensionHelpers;
using Sfa.Das.Sas.Web.ViewModels;
using Sfa.Das.Sas.Web.Views.Provider;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Views.Provider
{
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
                Website = Website
            };
        }
    }
}
