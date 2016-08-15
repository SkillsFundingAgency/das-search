using System;
using System.Reflection;

namespace Sfa.Das.Sas.ApprenticeshipInfoService.Web.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}