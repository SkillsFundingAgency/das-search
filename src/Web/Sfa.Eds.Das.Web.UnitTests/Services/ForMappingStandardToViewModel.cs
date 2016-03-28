namespace Sfa.Eds.Das.Web.UnitTests.Services
{
    using FluentAssertions;

    using NUnit.Framework;

    using Sfa.Eds.Das.Core.Domain.Model;
    using Sfa.Eds.Das.Web.Services;
    using Sfa.Eds.Das.Web.ViewModels;

    [TestFixture]
    public class ForMappingStandardToViewModel
    {
        [Test]
        public void WhenTypicalLengthIsEmpty()
        {
            MappingService mappingService = new MappingService(null);

            var standardResult = new Standard { Title = "item 1" };

            var viewModel = mappingService.Map<Standard, StandardViewModel>(standardResult);

            viewModel.TypicalLengthMessage.Should().BeEmpty();
        }

        [Test]
        public void WhenTypicalLengthIsIsFixed()
        {
            MappingService mappingService = new MappingService(null);

            var tl1 = new TypicalLength { From = 12, To = 0, Unit = "m" };
            var tl2 = new TypicalLength { From = 12, To = 12, Unit = "m" };
            var tl3 = new TypicalLength { From = 0, To = 12, Unit = "m" };

            var standardResultFrom = new Standard { Title = "item 1", TypicalLength = tl1 };
            var standardResultSame = new Standard { Title = "item 2", TypicalLength = tl2 };
            var standardResultTo = new Standard { Title = "item 3", TypicalLength = tl3 };

            var viewModel1 = mappingService.Map<Standard, StandardViewModel>(standardResultFrom);
            var viewModel2 = mappingService.Map<Standard, StandardViewModel>(standardResultSame);
            var viewModel3 = mappingService.Map<Standard, StandardViewModel>(standardResultTo);

            viewModel1.TypicalLengthMessage.ShouldBeEquivalentTo("12 months");
            viewModel2.TypicalLengthMessage.ShouldBeEquivalentTo("12 months");
            viewModel3.TypicalLengthMessage.ShouldBeEquivalentTo("12 months");
        }

        [Test]
        public void WhenTypicalLengthIsIsRange()
        {
            MappingService mappingService = new MappingService(null);

            var tl1 = new TypicalLength { From = 12, To = 24, Unit = "m" };

            var standardResultFrom = new Standard { Title = "item 1", TypicalLength = tl1 };

            var viewModel1 = mappingService.Map<Standard, StandardViewModel>(standardResultFrom);

            viewModel1.TypicalLengthMessage.ShouldBeEquivalentTo("12 to 24 months");
        }

        [Test]
        public void WhenTypicalLengthIsIsIligal()
        {
            MappingService mappingService = new MappingService(null);

            var tl1 = new TypicalLength { From = 12, To = 6, Unit = "m" };
            var tl2 = new TypicalLength { From = 12, To = 12, Unit = null };
            var tl3 = new TypicalLength { From = 0, To = 0, Unit = "m" };

            var standardResultFrom = new Standard { Title = "item 1", TypicalLength = tl1 };
            var standardResultNullUnit = new Standard { Title = "item 2", TypicalLength = tl2 };
            var standardResultZero = new Standard { Title = "item 2", TypicalLength = tl3 };

            var viewModel1 = mappingService.Map<Standard, StandardViewModel>(standardResultFrom);
            var viewModel2 = mappingService.Map<Standard, StandardViewModel>(standardResultNullUnit);
            var viewModel3 = mappingService.Map<Standard, StandardViewModel>(standardResultZero);

            viewModel1.TypicalLengthMessage.Should().BeEmpty();
            viewModel2.TypicalLengthMessage.ShouldBeEquivalentTo("12 ");
            viewModel3.TypicalLengthMessage.Should().BeEmpty();
        }

        [Test]
        public void WhenIntroductionFieldsContainAList()
        {
            MappingService mappingService = new MappingService(null);

            var standardResult = new Standard
            {
                Title = "item 1",
                IntroductoryText = "- item 1\n- item 2  \n\nSome text"
            };

            var viewModel = mappingService.Map<Standard, StandardViewModel>(standardResult);

            var textFormated = viewModel.IntroductoryTextHtml.Replace("\r", string.Empty).Replace("\n", string.Empty);

            textFormated.ShouldBeEquivalentTo("<ul><li>item 1</li><li>item 2</li></ul><p>Some text</p>");
        }

        [Test]
        public void WhenPdfLinkIsMissing()
        {
            MappingService mappingService = new MappingService(null);

            var standardResult = new Standard
            {
                Title = "item 1",
                StandardPdf = null,
                AssessmentPlanPdf = null
            };

            var viewModel = mappingService.Map<Standard, StandardViewModel>(standardResult);

            viewModel.StandardPdfUrlTitle.Should().BeEmpty();
            viewModel.AssessmentPlanPdfUrlTitle.Should().BeEmpty();
        }

        [Test]
        public void PdfLinkShouldHaveTitle()
        {
            MappingService mappingService = new MappingService(null);

            var standardResult = new Standard
            {
                Title = "item 1",
                StandardPdf = "httt://www.sfatest.co.uk/path/to/file_standard_sep_14.pdf",
                AssessmentPlanPdf = null
            };

            var viewModel = mappingService.Map<Standard, StandardViewModel>(standardResult);

            viewModel.StandardPdfUrlTitle.Should().BeEquivalentTo("file standard sep 14");
            viewModel.AssessmentPlanPdfUrlTitle.Should().BeEmpty();
        }
    }
}
