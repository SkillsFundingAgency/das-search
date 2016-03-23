﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sfa.Eds.Das.Web.Views.Standard
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Sfa.Eds.Das.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Standard/_SearchResultMessage.cshtml")]
    public partial class SearchResultMessage : System.Web.Mvc.WebViewPage<Sfa.Eds.Das.Web.ViewModels.ApprenticeshipSearchResultViewModel>
    {

#line 25 "..\..\Views\Standard\_SearchResultMessage.cshtml"
public System.Web.WebPages.HelperResult  RenderErrorMessage()
{
#line default
#line hidden
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {

#line 26 "..\..\Views\Standard\_SearchResultMessage.cshtml"
 


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "    <p>\r\n        There was a problem performing a search. Try again later.\r\n    <" +
"/p>\r\n");


#line 30 "..\..\Views\Standard\_SearchResultMessage.cshtml"


#line default
#line hidden
});

#line 30 "..\..\Views\Standard\_SearchResultMessage.cshtml"
}
#line default
#line hidden

#line 31 "..\..\Views\Standard\_SearchResultMessage.cshtml"
public System.Web.WebPages.HelperResult RenderZeroResult()
{
#line default
#line hidden
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {

#line 32 "..\..\Views\Standard\_SearchResultMessage.cshtml"
 


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "    <p>\r\n        There are no apprenticeships matching your search for \'<b>");


#line 34 "..\..\Views\Standard\_SearchResultMessage.cshtml"
                                    WriteTo(__razor_helper_writer, Model.SearchTerm);


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "</b>\'.\r\n    </p>\r\n");


#line 36 "..\..\Views\Standard\_SearchResultMessage.cshtml"


#line default
#line hidden
});

#line 36 "..\..\Views\Standard\_SearchResultMessage.cshtml"
}
#line default
#line hidden

#line 37 "..\..\Views\Standard\_SearchResultMessage.cshtml"
public System.Web.WebPages.HelperResult  RenderAllResult()
{
#line default
#line hidden
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {

#line 38 "..\..\Views\Standard\_SearchResultMessage.cshtml"
 


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "    <p>\r\n        All apprenticeships.\r\n    </p>\r\n");


#line 42 "..\..\Views\Standard\_SearchResultMessage.cshtml"


#line default
#line hidden
});

#line 42 "..\..\Views\Standard\_SearchResultMessage.cshtml"
}
#line default
#line hidden

#line 43 "..\..\Views\Standard\_SearchResultMessage.cshtml"
public System.Web.WebPages.HelperResult  RenderMessageOneResult()
{
#line default
#line hidden
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {

#line 44 "..\..\Views\Standard\_SearchResultMessage.cshtml"
 


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "    <p>\r\n        There is <b>");


#line 46 "..\..\Views\Standard\_SearchResultMessage.cshtml"
WriteTo(__razor_helper_writer, Model.TotalResults);


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "</b> apprenticeship matching your search for \'<b>");


#line 46 "..\..\Views\Standard\_SearchResultMessage.cshtml"
                                                          WriteTo(__razor_helper_writer, Model.SearchTerm);


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "</b>\'.\r\n    </p>\r\n");


#line 48 "..\..\Views\Standard\_SearchResultMessage.cshtml"


#line default
#line hidden
});

#line 48 "..\..\Views\Standard\_SearchResultMessage.cshtml"
}
#line default
#line hidden

#line 50 "..\..\Views\Standard\_SearchResultMessage.cshtml"
public System.Web.WebPages.HelperResult  RenderMessage()
{
#line default
#line hidden
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {

#line 51 "..\..\Views\Standard\_SearchResultMessage.cshtml"
 


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "    <p>\r\n        There are <b>");


#line 53 "..\..\Views\Standard\_SearchResultMessage.cshtml"
WriteTo(__razor_helper_writer, Model.TotalResults);


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "</b> apprenticeships matching your search for \'<b>");


#line 53 "..\..\Views\Standard\_SearchResultMessage.cshtml"
                                                            WriteTo(__razor_helper_writer, Model.SearchTerm);


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "</b>\'.\r\n    </p>\r\n");


#line 55 "..\..\Views\Standard\_SearchResultMessage.cshtml"


#line default
#line hidden
});

#line 55 "..\..\Views\Standard\_SearchResultMessage.cshtml"
}
#line default
#line hidden

        public SearchResultMessage()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div>\r\n");

            
            #line 4 "..\..\Views\Standard\_SearchResultMessage.cshtml"
    
            
            #line default
            #line hidden
            
            #line 4 "..\..\Views\Standard\_SearchResultMessage.cshtml"
     if (Model.HasError)
    {
        
            
            #line default
            #line hidden
            
            #line 6 "..\..\Views\Standard\_SearchResultMessage.cshtml"
   Write(RenderErrorMessage());

            
            #line default
            #line hidden
            
            #line 6 "..\..\Views\Standard\_SearchResultMessage.cshtml"
                             
    }
    else if (Model.TotalResults == 0)
    {
        
            
            #line default
            #line hidden
            
            #line 10 "..\..\Views\Standard\_SearchResultMessage.cshtml"
   Write(RenderZeroResult());

            
            #line default
            #line hidden
            
            #line 10 "..\..\Views\Standard\_SearchResultMessage.cshtml"
                           ;
    }
    else if (string.IsNullOrEmpty(Model.SearchTerm) || Model.SearchTerm == "*")
    {
        
            
            #line default
            #line hidden
            
            #line 14 "..\..\Views\Standard\_SearchResultMessage.cshtml"
   Write(RenderAllResult());

            
            #line default
            #line hidden
            
            #line 14 "..\..\Views\Standard\_SearchResultMessage.cshtml"
                          
    }
    else if (Model.TotalResults == 1)
    {
        
            
            #line default
            #line hidden
            
            #line 18 "..\..\Views\Standard\_SearchResultMessage.cshtml"
   Write(RenderMessageOneResult());

            
            #line default
            #line hidden
            
            #line 18 "..\..\Views\Standard\_SearchResultMessage.cshtml"
                                 
    }
    else
    {
        
            
            #line default
            #line hidden
            
            #line 22 "..\..\Views\Standard\_SearchResultMessage.cshtml"
   Write(RenderMessage());

            
            #line default
            #line hidden
            
            #line 22 "..\..\Views\Standard\_SearchResultMessage.cshtml"
                        
    }

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n");

WriteLiteral("\r\n");

        }
    }
}
#pragma warning restore 1591
