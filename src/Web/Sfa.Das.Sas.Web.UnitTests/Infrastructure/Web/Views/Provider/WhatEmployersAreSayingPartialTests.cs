namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Views.Provider
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;
    using SFA.DAS.Apprenticeships.Api.Types.Providers;
    using Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.ExtensionHelpers;
    using Sfa.Das.Sas.Web.ViewModels;
    using Sfa.Das.Sas.Web.Views.Provider;

    [TestFixture]
    [Parallelizable]
    public class WhatEmployersAreSayingPartialTests : ViewTestBase
    {
        private static FeedbackViewModel _providerFeedback;

        [OneTimeSetUp]
        public void Setup()
        {
            _providerFeedback = GetProviderFeedback();
        }

        [Test]
        [Order(1)]
        public void ShoulNotShowFeedbackIfNotSet()
        {
            var providerDetails = new WhatEmployersAreSaying();

            var html = providerDetails.RenderAsHtml(null).ToAngleSharp();
            GetPartial(html, "#feedback-heading").Should().Be(string.Empty);
        }

        [Test]
        [Order(1)]
        public void ShouldShowFeedbackRatingsIfFeedbackSet()
        {
            var providerDetails = new WhatEmployersAreSaying();

            var html = providerDetails.RenderAsHtml(_providerFeedback).ToAngleSharp();
            GetPartial(html, "#feedback-heading").Should().Contain("Based on 34 reviews");
            GetPartial(html, "#strengths").Should().Contain("Strengths");
            GetPartial(html, "#weaknesses").Should().Contain("Things to improve");
        }

        [Test]
        [Order(1)]
        public void ShoulShowStrengthsIfStrengthsSet()
        {
            var providerDetails = new WhatEmployersAreSaying();

            var html = providerDetails.RenderAsHtml(_providerFeedback).ToAngleSharp();
            GetHtmlElement(html, "#strengths").GetElementsByTagName("li").First().InnerHtml.Should().Be("Strength (6)");
        }

        [Test]
        [Order(2)]
        public void ShoulNotShowStrengthsIfNoStrengthsSet()
        {
            var providerDetails = new WhatEmployersAreSaying();
            _providerFeedback.Strengths.Clear();

            var html = providerDetails.RenderAsHtml(_providerFeedback).ToAngleSharp();
            GetPartial(html, "#strengths").Should().NotContain("Strengths");
        }

        [Test]
        [Order(1)]
        public void ShoulShowWeaknessesIfWeaknessesSet()
        {
            var providerDetails = new WhatEmployersAreSaying();

            var html = providerDetails.RenderAsHtml(_providerFeedback).ToAngleSharp();
            GetHtmlElement(html, "#weaknesses").GetElementsByTagName("li").First().InnerHtml.Should().Contain("Weaknesss (4)");
        }

        [Test]
        [Order(2)]
        public void ShoulNotShowWeaknessesIfNoWeaknessesSet()
        {
            var providerDetails = new WhatEmployersAreSaying();
            var providerFeedback = GetProviderFeedback();
            providerFeedback.Weaknesses.Clear();

            var html = providerDetails.RenderAsHtml(providerFeedback).ToAngleSharp();
            GetPartial(html, "#weaknesses").Should().NotContain("Things to improve");
        }

        private static FeedbackViewModel GetProviderFeedback()
        {
            return new FeedbackViewModel
            {
                ExcellentFeedbackCount = 10,
                GoodFeedbackCount = 9,
                PoorFeedbackCount = 8,
                VeryPoorFeedbackCount = 7,
                Strengths = new List<ProviderAttribute> { new ProviderAttribute { Name = "Strength", Count = 6 } },
                Weaknesses = new List<ProviderAttribute> { new ProviderAttribute { Name = "Weaknesss", Count = 4 } }
            };
        }
    }
}
