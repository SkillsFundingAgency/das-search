using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Services
{
    [TestFixture]
    public class ForMappingGetStandardResponseToViewModel
    {
        [Test]
        public void WhenTypicalLengthIsEmpty()
        {
            MappingService mappingService = new MappingService(null);

            var standardResult = new GetStandardResponse { Standard = new Standard { Title = "item 1" } };

            var viewModel = mappingService.Map<GetStandardResponse, StandardViewModel>(standardResult);

            viewModel.TypicalLengthMessage.Should().BeEmpty();
        }

        [Test]
        public void WhenTypicalLengthIsIsFixed()
        {
            MappingService mappingService = new MappingService(null);

            var tl1 = new TypicalLength { From = 12, To = 0, Unit = "m" };
            var tl2 = new TypicalLength { From = 12, To = 12, Unit = "m" };
            var tl3 = new TypicalLength { From = 0, To = 12, Unit = "m" };

            var standardResultFrom = new GetStandardResponse { Standard = new Standard { Title = "item 1", TypicalLength = tl1 } };
            var standardResultSame = new GetStandardResponse { Standard = new Standard { Title = "item 2", TypicalLength = tl2 } };
            var standardResultTo = new GetStandardResponse { Standard = new Standard { Title = "item 3", TypicalLength = tl3 } };

            var viewModel1 = mappingService.Map<GetStandardResponse, StandardViewModel>(standardResultFrom);
            var viewModel2 = mappingService.Map<GetStandardResponse, StandardViewModel>(standardResultSame);
            var viewModel3 = mappingService.Map<GetStandardResponse, StandardViewModel>(standardResultTo);

            viewModel1.TypicalLengthMessage.ShouldBeEquivalentTo("12 months");
            viewModel2.TypicalLengthMessage.ShouldBeEquivalentTo("12 months");
            viewModel3.TypicalLengthMessage.ShouldBeEquivalentTo("12 months");
        }

        [Test]
        public void WhenTypicalLengthIsIsRange()
        {
            MappingService mappingService = new MappingService(null);

            var tl1 = new TypicalLength { From = 12, To = 24, Unit = "m" };

            var standardResultFrom = new GetStandardResponse {Standard = new Standard { Title = "item 1", TypicalLength = tl1 } };

            var viewModel1 = mappingService.Map<GetStandardResponse, StandardViewModel>(standardResultFrom);

            viewModel1.TypicalLengthMessage.ShouldBeEquivalentTo("12 to 24 months");
        }

        [Test]
        public void WhenTypicalLengthIsIsIligal()
        {
            MappingService mappingService = new MappingService(null);

            var tl1 = new TypicalLength { From = 12, To = 6, Unit = "m" };
            var tl2 = new TypicalLength { From = 12, To = 12, Unit = null };
            var tl3 = new TypicalLength { From = 0, To = 0, Unit = "m" };

            var standardResultFrom = new GetStandardResponse { Standard = new Standard { Title = "item 1", TypicalLength = tl1 } };
            var standardResultNullUnit = new GetStandardResponse { Standard = new Standard { Title = "item 2", TypicalLength = tl2 } };
            var standardResultZero = new GetStandardResponse { Standard = new Standard { Title = "item 2", TypicalLength = tl3 } };

            var viewModel1 = mappingService.Map<GetStandardResponse, StandardViewModel>(standardResultFrom);
            var viewModel2 = mappingService.Map<GetStandardResponse, StandardViewModel>(standardResultNullUnit);
            var viewModel3 = mappingService.Map<GetStandardResponse, StandardViewModel>(standardResultZero);

            viewModel1.TypicalLengthMessage.Should().BeEmpty();
            viewModel2.TypicalLengthMessage.ShouldBeEquivalentTo("12 ");
            viewModel3.TypicalLengthMessage.Should().BeEmpty();
        }
    }
}
