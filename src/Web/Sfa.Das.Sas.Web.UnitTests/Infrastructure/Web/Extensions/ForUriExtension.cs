using System;
using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.Web.Extensions;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Extensions
{
    [TestFixture]
    public class ForUriExtension
    {
        [Test]
        public void WhenPreviousSearchIsUnknown()
        {
            var uri = new Uri("http://www.sfatest.co.uk/path/to/page");
            var resultUrl = uri.GetSearchResultUrl("action/url").Url;
            var resultTitle = uri.GetSearchResultUrl("action/url").Title;

            resultUrl.ShouldBeEquivalentTo("action/url");
            resultTitle.ShouldBeEquivalentTo("Back to search page");
        }

        [Test]
        public void WhenPreviousSearchIsKnown()
        {
            var uri = new Uri("http://www.sfatest.co.uk/path/to/page?keywords=hello+world");
            var resultUrl = uri.GetSearchResultUrl("action/url").Url;
            var resultTitle = uri.GetSearchResultUrl("action/url").Title;

            resultUrl.ShouldBeEquivalentTo("http://www.sfatest.co.uk/path/to/page?keywords=hello+world");
            resultTitle.ShouldBeEquivalentTo("Back");
        }

        [Test]
        public void WhenPreviousProviderSearchIsKnown()
        {
            var uri = new Uri("http://www.sfatest.co.uk/path/to/page?apprenticeshipid=8&postcode=CV212BB");
            var resultUrl = uri.GetProviderSearchResultBackUrl("action/url").Url;
            var resultTitle = uri.GetProviderSearchResultBackUrl("action/url").Title;

            resultUrl.ShouldBeEquivalentTo("http://www.sfatest.co.uk/path/to/page?apprenticeshipid=8&postcode=CV212BB");
            resultTitle.ShouldBeEquivalentTo("Back");
        }

        [Test]
        public void WhenPreviousStandardIdIsUnknown()
        {
            var uri = new Uri("http://www.sfatest.co.uk/path/to/page?postcode=CV212BB");
            var resultUrl = uri.GetProviderSearchResultBackUrl("action/url").Url;
            var resultTitle = uri.GetProviderSearchResultBackUrl("action/url").Title;

            resultUrl.ShouldBeEquivalentTo("action/url");
            resultTitle.ShouldBeEquivalentTo("Back to search page");
        }

        [Test]
        public void WhenPreviousPostcodeIsUnknown()
        {
            var uri = new Uri("http://www.sfatest.co.uk/path/to/page?standardid=8");
            var resultUrl = uri.GetProviderSearchResultBackUrl("action/url").Url;
            var resultTitle = uri.GetProviderSearchResultBackUrl("action/url").Title;

            resultUrl.ShouldBeEquivalentTo("action/url");
            resultTitle.ShouldBeEquivalentTo("Back to search page");
        }
    }
}