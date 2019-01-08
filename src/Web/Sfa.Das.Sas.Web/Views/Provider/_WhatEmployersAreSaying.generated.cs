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

namespace Sfa.Das.Sas.Web.Views.Provider
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Provider/_WhatEmployersAreSaying.cshtml")]
    public partial class WhatEmployersAreSaying : System.Web.Mvc.WebViewPage<Sfa.Das.Sas.Web.ViewModels.FeedbackViewModel>
    {
        public WhatEmployersAreSaying()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
 if (Model != null && Model.TotalFeedbackCount > 0)
{

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" id=\"feedback-heading\"");

WriteLiteral(" class=\"graph-heading-group\"");

WriteLiteral(">\r\n        <h2");

WriteLiteral(" class=\"heading-medium\"");

WriteLiteral(">What employers are saying</h2>\r\n        <p");

WriteLiteral(" class=\"meta\"");

WriteLiteral(">Based on ");

            
            #line 7 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
                            Write(Model.TotalFeedbackCount);

            
            #line default
            #line hidden
WriteLiteral(" reviews from employers that work with this training provider.</p>\r\n        <p");

WriteLiteral(" class=\"meta\"");

WriteLiteral("><a");

WriteLiteral(" href=\"https://sfadigital.blog.gov.uk/2018/09/26/new-feedback-feature-for-the-app" +
"renticeship-service/\"");

WriteLiteral(" target=\"_blank\"");

WriteLiteral(">Find out how these reviews work</a></p>\r\n    </div>\r\n");

            
            #line 10 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"


            
            #line default
            #line hidden
WriteLiteral("    <h3");

WriteLiteral(" class=\"heading-small\"");

WriteLiteral(">Overall rating from October 2018 to now</h3>\r\n");

WriteLiteral("    <ul");

WriteLiteral(" class=\"graph-list\"");

WriteLiteral(">\r\n        <li");

WriteLiteral(" class=\"level-1\"");

WriteLiteral(">\r\n            <span");

WriteLiteral(" class=\"label\"");

WriteLiteral(">Excellent</span>\r\n            <span");

WriteLiteral(" class=\"chart\"");

WriteLiteral(">\r\n                <span");

WriteLiteral(" class=\"graph\"");

WriteLiteral(">\r\n                    <span");

WriteLiteral(" class=\"bar\"");

WriteAttribute("style", Tuple.Create(" style=\"", 859), Tuple.Create("\"", 909)
, Tuple.Create(Tuple.Create("", 867), Tuple.Create("width:", 867), true)
            
            #line 17 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
, Tuple.Create(Tuple.Create(" ", 873), Tuple.Create<System.Object, System.Int32>(Model.ExcellentFeedbackPercentage
            
            #line default
            #line hidden
, 874), false)
, Tuple.Create(Tuple.Create("", 908), Tuple.Create("%", 908), true)
);

WriteLiteral("></span>\r\n                </span>\r\n            </span>\r\n            <span");

WriteLiteral(" class=\"figure\"");

WriteLiteral(">");

            
            #line 20 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
                            Write(Model.ExcellentFeedbackCount);

            
            #line default
            #line hidden
WriteLiteral(" <span");

WriteLiteral(" class=\"visually-hidden\"");

WriteLiteral(">out of ");

            
            #line 20 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
                                                                                               Write(Model.TotalFeedbackCount);

            
            #line default
            #line hidden
WriteLiteral(" </span>reviews</span>\r\n        </li>\r\n        <li");

WriteLiteral(" class=\"level-2\"");

WriteLiteral(">\r\n            <span");

WriteLiteral(" class=\"label\"");

WriteLiteral(">Good</span>\r\n            <span");

WriteLiteral(" class=\"chart\"");

WriteLiteral(">\r\n                <span");

WriteLiteral(" class=\"graph\"");

WriteLiteral(">\r\n                    <span");

WriteLiteral(" class=\"bar\"");

WriteAttribute("style", Tuple.Create(" style=\"", 1314), Tuple.Create("\"", 1359)
, Tuple.Create(Tuple.Create("", 1322), Tuple.Create("width:", 1322), true)
            
            #line 26 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
, Tuple.Create(Tuple.Create(" ", 1328), Tuple.Create<System.Object, System.Int32>(Model.GoodFeedbackPercentage
            
            #line default
            #line hidden
, 1329), false)
, Tuple.Create(Tuple.Create("", 1358), Tuple.Create("%", 1358), true)
);

WriteLiteral("></span>\r\n                </span>\r\n            </span>\r\n            <span");

WriteLiteral(" class=\"figure\"");

WriteLiteral(">");

            
            #line 29 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
                            Write(Model.GoodFeedbackCount);

            
            #line default
            #line hidden
WriteLiteral(" <span");

WriteLiteral(" class=\"visually-hidden\"");

WriteLiteral(">out of ");

            
            #line 29 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
                                                                                          Write(Model.TotalFeedbackCount);

            
            #line default
            #line hidden
WriteLiteral(" </span>reviews</span>\r\n        </li>\r\n        <li");

WriteLiteral(" class=\"level-3\"");

WriteLiteral(">\r\n            <span");

WriteLiteral(" class=\"label\"");

WriteLiteral(">Poor</span>\r\n            <span");

WriteLiteral(" class=\"chart\"");

WriteLiteral(">\r\n                <span");

WriteLiteral(" class=\"graph\"");

WriteLiteral(">\r\n                    <span");

WriteLiteral(" class=\"bar\"");

WriteAttribute("style", Tuple.Create(" style=\"", 1759), Tuple.Create("\"", 1804)
, Tuple.Create(Tuple.Create("", 1767), Tuple.Create("width:", 1767), true)
            
            #line 35 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
, Tuple.Create(Tuple.Create(" ", 1773), Tuple.Create<System.Object, System.Int32>(Model.PoorFeedbackPercentage
            
            #line default
            #line hidden
, 1774), false)
, Tuple.Create(Tuple.Create("", 1803), Tuple.Create("%", 1803), true)
);

WriteLiteral("></span>\r\n                </span>\r\n            </span>\r\n            <span");

WriteLiteral(" class=\"figure\"");

WriteLiteral(">");

            
            #line 38 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
                            Write(Model.PoorFeedbackCount);

            
            #line default
            #line hidden
WriteLiteral(" <span");

WriteLiteral(" class=\"visually-hidden\"");

WriteLiteral(">out of ");

            
            #line 38 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
                                                                                          Write(Model.TotalFeedbackCount);

            
            #line default
            #line hidden
WriteLiteral(" </span>reviews</span>\r\n        </li>\r\n        <li");

WriteLiteral(" class=\"level-4\"");

WriteLiteral(">\r\n            <span");

WriteLiteral(" class=\"label\"");

WriteLiteral(">Very poor</span>\r\n            <span");

WriteLiteral(" class=\"chart\"");

WriteLiteral(">\r\n                <span");

WriteLiteral(" class=\"graph\"");

WriteLiteral(">\r\n                    <span");

WriteLiteral(" class=\"bar\"");

WriteAttribute("style", Tuple.Create(" style=\"", 2209), Tuple.Create("\"", 2258)
, Tuple.Create(Tuple.Create("", 2217), Tuple.Create("width:", 2217), true)
            
            #line 44 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
, Tuple.Create(Tuple.Create(" ", 2223), Tuple.Create<System.Object, System.Int32>(Model.VeryPoorFeedbackPercentage
            
            #line default
            #line hidden
, 2224), false)
, Tuple.Create(Tuple.Create("", 2257), Tuple.Create("%", 2257), true)
);

WriteLiteral("></span>\r\n                </span>\r\n            </span>\r\n            <span");

WriteLiteral(" class=\"figure\"");

WriteLiteral(">");

            
            #line 47 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
                            Write(Model.VeryPoorFeedbackCount);

            
            #line default
            #line hidden
WriteLiteral(" <span");

WriteLiteral(" class=\"visually-hidden\"");

WriteLiteral(">out of ");

            
            #line 47 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
                                                                                              Write(Model.TotalFeedbackCount);

            
            #line default
            #line hidden
WriteLiteral(" </span>reviews</span>\r\n        </li>\r\n    </ul>\r\n");

WriteLiteral("    <div");

WriteLiteral(" class=\"grid-row provider-list-details\"");

WriteLiteral(">\r\n");

            
            #line 51 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
        
            
            #line default
            #line hidden
            
            #line 51 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
         if (Model.Strengths.Any())
        {

            
            #line default
            #line hidden
WriteLiteral("            <div");

WriteLiteral(" id=\"strengths\"");

WriteLiteral(" class=\"column-half\"");

WriteLiteral(">\r\n                <h4");

WriteLiteral(" class=\"heading-small\"");

WriteLiteral(">Strengths</h4>\r\n                <ul");

WriteLiteral(" class=\"list\"");

WriteLiteral(">\r\n");

            
            #line 56 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 56 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
                     foreach (var strength in Model.Strengths)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <li>");

            
            #line 58 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
                       Write(strength.Name);

            
            #line default
            #line hidden
WriteLiteral(" (");

            
            #line 58 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
                                       Write(strength.Count);

            
            #line default
            #line hidden
WriteLiteral(")</li>\r\n");

            
            #line 59 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                </ul>\r\n            </div>\r\n");

            
            #line 62 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("        ");

            
            #line 63 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
         if (Model.Weaknesses.Any())
        {

            
            #line default
            #line hidden
WriteLiteral("            <div");

WriteLiteral(" id=\"weaknesses\"");

WriteLiteral(" class=\"column-half\"");

WriteLiteral(">\r\n                <h4");

WriteLiteral(" class=\"heading-small\"");

WriteLiteral(">Things to improve</h4>\r\n                <ul");

WriteLiteral(" class=\"list\"");

WriteLiteral(">\r\n");

            
            #line 68 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 68 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
                     foreach (var toImprove in Model.Weaknesses)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <li>");

            
            #line 70 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
                       Write(toImprove.Name);

            
            #line default
            #line hidden
WriteLiteral(" (");

            
            #line 70 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
                                        Write(toImprove.Count);

            
            #line default
            #line hidden
WriteLiteral(")</li>\r\n");

            
            #line 71 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                </ul>\r\n            </div>\r\n");

            
            #line 74 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n");

            
            #line 76 "..\..\Views\Provider\_WhatEmployersAreSaying.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n");

        }
    }
}
#pragma warning restore 1591