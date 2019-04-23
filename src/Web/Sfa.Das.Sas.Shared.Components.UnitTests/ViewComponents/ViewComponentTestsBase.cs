using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using Sfa.Das.Sas.Shared.Components.Domain.Interfaces;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;

namespace Sfa.Das.Sas.Shared.Components.UnitTests.ViewComponents.Fat
{

    public class ViewComponentTestsBase
    {
        internal FatSearchViewComponent _sut;
        protected Mock<ICssClasses> _cssClasses;

        protected ViewComponentContext _viewComponentContext;

        public void Setup()
        {
            var httpContext = new DefaultHttpContext();
            var viewContext = new ViewContext();
            viewContext.HttpContext = httpContext;
            _viewComponentContext = new ViewComponentContext();
            _viewComponentContext.ViewContext = viewContext;

            _cssClasses = new Mock<ICssClasses>(MockBehavior.Strict);
            _cssClasses.Setup(s => s.ClassModifier).Returns("");
        }

        
      
    }
}
