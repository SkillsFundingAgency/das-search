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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Provider/_ManageApprenticeshipFunds.cshtml")]
    public partial class ManageApprenticeshipFunds : System.Web.Mvc.WebViewPage<Sfa.Das.Sas.Web.ViewModels.ManageApprenticeshipFundsViewModel>
    {
        public ManageApprenticeshipFunds()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\Provider\_ManageApprenticeshipFunds.cshtml"
 if (Model.IsLevyPayer)
{

            
            #line default
            #line hidden
WriteLiteral("    <a");

WriteLiteral(" id=\"register_to_manage\"");

WriteAttribute("href", Tuple.Create(" href=\"", 130), Tuple.Create("\"", 147)
            
            #line 5 "..\..\Views\Provider\_ManageApprenticeshipFunds.cshtml"
, Tuple.Create(Tuple.Create("", 137), Tuple.Create<System.Object, System.Int32>(Model.Url
            
            #line default
            #line hidden
, 137), false)
);

WriteLiteral(" style=\"float: right; margin-top: 10px;\"");

WriteLiteral(" rel=\"external\"");

WriteLiteral(" target=\"_blank\"");

WriteLiteral(">Use your available apprenticeship funds</a>\r\n");

            
            #line 6 "..\..\Views\Provider\_ManageApprenticeshipFunds.cshtml"
}
            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
