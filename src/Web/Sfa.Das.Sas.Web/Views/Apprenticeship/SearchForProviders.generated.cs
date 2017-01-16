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

namespace Sfa.Das.Sas.Web.Views.Apprenticeship
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
    using Sfa.Das.Sas.Web;
    
    #line 1 "..\..\Views\Apprenticeship\SearchForProviders.cshtml"
    using Sfa.Das.Sas.Web.Extensions;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Apprenticeship/SearchForProviders.cshtml")]
    public partial class SearchForProviders : System.Web.Mvc.WebViewPage<Sfa.Das.Sas.Web.ViewModels.ProviderSearchViewModel>
    {

#line 60 "..\..\Views\Apprenticeship\SearchForProviders.cshtml"
public System.Web.WebPages.HelperResult ShowErrorMessage(bool hasError, bool wrongPostcode, string postcodeCountry)
{
#line default
#line hidden
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {

#line 61 "..\..\Views\Apprenticeship\SearchForProviders.cshtml"
 
    if(hasError)
    {


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "        <span");

WriteLiteralTo(__razor_helper_writer, " class=\"error-message\"");

WriteLiteralTo(__razor_helper_writer, ">\r\n            Sorry, postcode search not working, please try again later\r\n      " +
"  </span>\r\n");


#line 67 "..\..\Views\Apprenticeship\SearchForProviders.cshtml"
    }
    else if (wrongPostcode)
    {


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "        <span");

WriteLiteralTo(__razor_helper_writer, " class=\"error-message\"");

WriteLiteralTo(__razor_helper_writer, ">\r\n            You must enter a full and valid postcode\r\n        </span>\r\n");


#line 73 "..\..\Views\Apprenticeship\SearchForProviders.cshtml"
    }
    else if (postcodeCountry != null)
    {
        switch (postcodeCountry.ToLower())
        {
            case "wales":


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "                <span");

WriteLiteralTo(__razor_helper_writer, " class=\"error-message\"");

WriteLiteralTo(__razor_helper_writer, ">\r\n                    Information about apprenticeships in <a");

WriteLiteralTo(__razor_helper_writer, " href=\"https://businesswales.gov.wales/skillsgateway/apprenticeships\"");

WriteLiteralTo(__razor_helper_writer, ">Wales</a>\r\n                </span>\r\n");


#line 82 "..\..\Views\Apprenticeship\SearchForProviders.cshtml"
                break;
            case "northernireland":


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "                <span");

WriteLiteralTo(__razor_helper_writer, " class=\"error-message\"");

WriteLiteralTo(__razor_helper_writer, ">\r\n                    Information about apprenticeships in <a");

WriteLiteralTo(__razor_helper_writer, " href=\"https://www.nibusinessinfo.co.uk/content/apprenticeships-employers\"");

WriteLiteralTo(__razor_helper_writer, ">Northern Ireland</a>\r\n                </span>\r\n");


#line 87 "..\..\Views\Apprenticeship\SearchForProviders.cshtml"
                break;
            case "scotland":


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "                <span");

WriteLiteralTo(__razor_helper_writer, " class=\"error-message\"");

WriteLiteralTo(__razor_helper_writer, ">\r\n                    Information about apprenticeships in <a");

WriteLiteralTo(__razor_helper_writer, " href=\"https://www.apprenticeships.scot/\"");

WriteLiteralTo(__razor_helper_writer, ">Scotland</a>\r\n                </span>\r\n");


#line 92 "..\..\Views\Apprenticeship\SearchForProviders.cshtml"
                break;
        }
    }


#line default
#line hidden
});

#line 95 "..\..\Views\Apprenticeship\SearchForProviders.cshtml"
}
#line default
#line hidden

        public SearchForProviders()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\Apprenticeship\SearchForProviders.cshtml"
  
    ViewBag.Title = "Search for providers";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<main");

WriteLiteral(" id=\"content\"");

WriteLiteral(">\r\n    <hgroup");

WriteLiteral(" class=\"hgroup\"");

WriteLiteral(">\r\n        <h1");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(">\r\n            Find a training provider\r\n        </h1>\r\n        <p");

WriteLiteral(" class=\"lede\"");

WriteLiteral(">\r\n            For <strong>");

            
            #line 13 "..\..\Views\Apprenticeship\SearchForProviders.cshtml"
                   Write(Model.Title);

            
            #line default
            #line hidden
WriteLiteral("</strong>:\r\n        </p>\r\n    </hgroup>\r\n\r\n    <div");

WriteLiteral(" class=\"grid-row\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"column-two-thirds\"");

WriteLiteral(">\r\n    \r\n            <form");

WriteAttribute("action", Tuple.Create(" action=\"", 485), Tuple.Create("\"", 508)
            
            #line 20 "..\..\Views\Apprenticeship\SearchForProviders.cshtml"
, Tuple.Create(Tuple.Create("", 494), Tuple.Create<System.Object, System.Int32>(Model.PostUrl
            
            #line default
            #line hidden
, 494), false)
);

WriteLiteral(" class=\"postcode-form search-box\"");

WriteLiteral(" method=\"get\"");

WriteLiteral(">\r\n\r\n                <div");

WriteAttribute("class", Tuple.Create(" class=\"", 580), Tuple.Create("\"", 780)
, Tuple.Create(Tuple.Create("", 588), Tuple.Create("form-elements", 588), true)
            
            #line 22 "..\..\Views\Apprenticeship\SearchForProviders.cshtml"
, Tuple.Create(Tuple.Create("  ", 601), Tuple.Create<System.Object, System.Int32>(Model.HasError || Model.WrongPostcode || Model.PostcodeCountry == "Wales" || Model.PostcodeCountry == "Scotland" || Model.PostcodeCountry == "NorthernIreland" ? " error" : ""
            
            #line default
            #line hidden
, 603), false)
);

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"heading-group\"");

WriteLiteral(">\r\n                        <h2");

WriteLiteral(" class=\"heading-medium\"");

WriteLiteral(">\r\n                            Enter a postcode\r\n                        </h2>\r\n " +
"                       <p>For example: \'SW1A 2AA\'</p>\r\n                    </div" +
">\r\n\r\n                    <label");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(" for=\"search-box\"");

WriteLiteral(">\r\n                        Enter the full postcode of your apprentice’s workplace" +
"\r\n                        \r\n");

WriteLiteral("                        ");

            
            #line 33 "..\..\Views\Apprenticeship\SearchForProviders.cshtml"
                   Write(ShowErrorMessage(Model.HasError, Model.WrongPostcode, Model.PostcodeCountry));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n                        \r\n                    </label>\r\n                    <" +
"div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                        <input");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" name=\"apprenticeshipid\"");

WriteLiteral(" class=\"text-box form-control\"");

WriteAttribute("value", Tuple.Create(" value=\"", 1525), Tuple.Create("\"", 1556)
            
            #line 38 "..\..\Views\Apprenticeship\SearchForProviders.cshtml"
                          , Tuple.Create(Tuple.Create("", 1533), Tuple.Create<System.Object, System.Int32>(Model.ApprenticeshipId
            
            #line default
            #line hidden
, 1533), false)
);

WriteLiteral(">\r\n                        <input");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" name=\"keywords\"");

WriteLiteral(" class=\"text-box form-control\"");

WriteAttribute("value", Tuple.Create(" value=\"", 1650), Tuple.Create("\"", 1676)
            
            #line 39 "..\..\Views\Apprenticeship\SearchForProviders.cshtml"
                  , Tuple.Create(Tuple.Create("", 1658), Tuple.Create<System.Object, System.Int32>(Model.SearchTerms
            
            #line default
            #line hidden
, 1658), false)
);

WriteLiteral(">\r\n                        <input");

WriteLiteral(" type=\"search\"");

WriteLiteral(" name=\"PostCode\"");

WriteAttribute("value", Tuple.Create(" value=\"", 1740), Tuple.Create("\"", 1780)
            
            #line 40 "..\..\Views\Apprenticeship\SearchForProviders.cshtml"
, Tuple.Create(Tuple.Create("", 1748), Tuple.Create<System.Object, System.Int32>(Model.PostCode.FormatPostcode()
            
            #line default
            #line hidden
, 1748), false)
);

WriteLiteral(" id=\"search-box\"");

WriteLiteral(" class=\"text-box form-control postcode-search-box\"");

WriteLiteral(" maxlength=\"200\"");

WriteLiteral(" placeholder=\"\"");

WriteLiteral(">\r\n                        <input");

WriteLiteral(" class=\"button margin-top-x2 postcode-search-button\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" value=\"Search\"");

WriteLiteral(">\r\n                    </div>\r\n                </div>\r\n            </form>\r\n     " +
"       \r\n            <div");

WriteLiteral(" class=\"notice\"");

WriteLiteral(">\r\n                <i");

WriteLiteral(" class=\"icon icon-important\"");

WriteLiteral(">\r\n                    <span");

WriteLiteral(" class=\"visuallyhidden\"");

WriteLiteral(">Warning</span>\r\n                </i>\r\n                <strong");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(@">
                    This service contains details of training providers who currently offer apprenticeships.
                    It will be updated in 2017 to reflect the new register of apprenticeship training providers.
                </strong>
            </div>

        </div>
    </div>
</main>

");

        }
    }
}
#pragma warning restore 1591
