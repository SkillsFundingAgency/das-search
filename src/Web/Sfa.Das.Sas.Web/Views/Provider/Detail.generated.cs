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
    
    #line 5 "..\..\Views\Provider\Detail.cshtml"
    using FeatureToggle.Core.Fluent;
    
    #line default
    #line hidden
    
    #line 4 "..\..\Views\Provider\Detail.cshtml"
    using Sfa.Das.Sas.ApplicationServices.FeatureToggles;
    
    #line default
    #line hidden
    
    #line 2 "..\..\Views\Provider\Detail.cshtml"
    using Sfa.Das.Sas.Resources;
    
    #line default
    #line hidden
    using Sfa.Das.Sas.Web;
    
    #line 1 "..\..\Views\Provider\Detail.cshtml"
    using Sfa.Das.Sas.Web.Extensions;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Views\Provider\Detail.cshtml"
    using Sfa.Das.Sas.Web.ViewModels;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Provider/Detail.cshtml")]
    public partial class Detail : System.Web.Mvc.WebViewPage<ApprenticeshipDetailsViewModel>
    {

#line 129 "..\..\Views\Provider\Detail.cshtml"
public System.Web.WebPages.HelperResult ShowTrainingLocation(string title)
{
#line default
#line hidden
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {

#line 130 "..\..\Views\Provider\Detail.cshtml"
 
if (@Model != null)
{


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "    <dt");

WriteLiteralTo(__razor_helper_writer, " class=\"training-location-title\"");

WriteLiteralTo(__razor_helper_writer, ">");


#line 133 "..\..\Views\Provider\Detail.cshtml"
          WriteTo(__razor_helper_writer, title);


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "</dt>\n");


#line 134 "..\..\Views\Provider\Detail.cshtml"

    if (@Model.DeliveryModes.Count == 1 && @Model.DeliveryModes.Contains("100PercentEmployer"))
    {


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "        <dd");

WriteLiteralTo(__razor_helper_writer, " id=\"training-location\"");

WriteLiteralTo(__razor_helper_writer, " class=\"training-location\"");

WriteLiteralTo(__razor_helper_writer, ">\n            Training takes place at your location\n        </dd>\n");


#line 140 "..\..\Views\Provider\Detail.cshtml"
    }
    else
    {


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "        <dd");

WriteLiteralTo(__razor_helper_writer, " id=\"training-location\"");

WriteLiteralTo(__razor_helper_writer, " class=\"training-location\"");

WriteLiteralTo(__razor_helper_writer, ">\n");

WriteLiteralTo(__razor_helper_writer, "            ");


#line 144 "..\..\Views\Provider\Detail.cshtml"
WriteTo(__razor_helper_writer, Model.LocationAddressLine);


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, " \n        </dd>\n");


#line 146 "..\..\Views\Provider\Detail.cshtml"
    }
}


#line default
#line hidden
});

#line 148 "..\..\Views\Provider\Detail.cshtml"
}
#line default
#line hidden

#line 150 "..\..\Views\Provider\Detail.cshtml"
public System.Web.WebPages.HelperResult GetStandardPropertyHtml(string title, string id, string item, bool hideIfEmpty = false)
{
#line default
#line hidden
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {

#line 151 "..\..\Views\Provider\Detail.cshtml"
 
if (!string.IsNullOrEmpty(item) || !hideIfEmpty)
{


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "    <dt");

WriteAttributeTo(__razor_helper_writer, "class", Tuple.Create(" class=\"", 6213), Tuple.Create("\"", 6230)

#line 154 "..\..\Views\Provider\Detail.cshtml"
, Tuple.Create(Tuple.Create("", 6221), Tuple.Create<System.Object, System.Int32>(id

#line default
#line hidden
, 6221), false)
, Tuple.Create(Tuple.Create("", 6224), Tuple.Create("-title", 6224), true)
);

WriteLiteralTo(__razor_helper_writer, ">");


#line 154 "..\..\Views\Provider\Detail.cshtml"
WriteTo(__razor_helper_writer, title);


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "</dt>\n");

WriteLiteralTo(__razor_helper_writer, "    <dd");

WriteAttributeTo(__razor_helper_writer, "id", Tuple.Create(" id=\"", 6251), Tuple.Create("\"", 6259)

#line 155 "..\..\Views\Provider\Detail.cshtml"
, Tuple.Create(Tuple.Create("", 6256), Tuple.Create<System.Object, System.Int32>(id

#line default
#line hidden
, 6256), false)
);

WriteAttributeTo(__razor_helper_writer, "class", Tuple.Create(" class=\"", 6260), Tuple.Create("\"", 6271)

#line 155 "..\..\Views\Provider\Detail.cshtml"
, Tuple.Create(Tuple.Create("", 6268), Tuple.Create<System.Object, System.Int32>(id

#line default
#line hidden
, 6268), false)
);

WriteLiteralTo(__razor_helper_writer, ">");


#line 155 "..\..\Views\Provider\Detail.cshtml"
WriteTo(__razor_helper_writer, Html.Raw(item));


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "</dd>\n");


#line 156 "..\..\Views\Provider\Detail.cshtml"
}


#line default
#line hidden
});

#line 157 "..\..\Views\Provider\Detail.cshtml"
}
#line default
#line hidden

#line 159 "..\..\Views\Provider\Detail.cshtml"
public System.Web.WebPages.HelperResult GetEmailPropertyHtml(string title, string id, string item, bool hideIfEmpty = false)
{
#line default
#line hidden
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {

#line 160 "..\..\Views\Provider\Detail.cshtml"
 
if (!string.IsNullOrEmpty(item) || !hideIfEmpty)
{


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "    <dt");

WriteLiteralTo(__razor_helper_writer, " class=\"email-title\"");

WriteLiteralTo(__razor_helper_writer, ">");


#line 163 "..\..\Views\Provider\Detail.cshtml"
WriteTo(__razor_helper_writer, title);


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "</dt>\n");

WriteLiteralTo(__razor_helper_writer, "    <dd");

WriteAttributeTo(__razor_helper_writer, "id", Tuple.Create(" id=\"", 6492), Tuple.Create("\"", 6500)

#line 164 "..\..\Views\Provider\Detail.cshtml"
, Tuple.Create(Tuple.Create("", 6497), Tuple.Create<System.Object, System.Int32>(id

#line default
#line hidden
, 6497), false)
);

WriteLiteralTo(__razor_helper_writer, " class=\"email\"");

WriteLiteralTo(__razor_helper_writer, "><a");

WriteAttributeTo(__razor_helper_writer, "href", Tuple.Create(" href=\"", 6518), Tuple.Create("\"", 6547)
, Tuple.Create(Tuple.Create("", 6525), Tuple.Create("mailto:", 6525), true)

#line 164 "..\..\Views\Provider\Detail.cshtml"
, Tuple.Create(Tuple.Create("", 6532), Tuple.Create<System.Object, System.Int32>(Html.Raw(item)

#line default
#line hidden
, 6532), false)
);

WriteLiteralTo(__razor_helper_writer, ">");


#line 164 "..\..\Views\Provider\Detail.cshtml"
                                  WriteTo(__razor_helper_writer, Html.Raw(item));


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "</a></dd>\n");


#line 165 "..\..\Views\Provider\Detail.cshtml"
}


#line default
#line hidden
});

#line 166 "..\..\Views\Provider\Detail.cshtml"
}
#line default
#line hidden

#line 168 "..\..\Views\Provider\Detail.cshtml"
public System.Web.WebPages.HelperResult GetStandardPropertyAsLinkHtml(string title, string cssClass, string classTitle, string classIdentifier, string url, string urlTitle = "")
{
#line default
#line hidden
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {

#line 169 "..\..\Views\Provider\Detail.cshtml"
 
    if (!string.IsNullOrEmpty(url))
    {
        var linkProtocol = url.StartsWith("http") ? string.Empty : "http://";


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "        <dt");

WriteAttributeTo(__razor_helper_writer, "class", Tuple.Create(" class=\"", 6858), Tuple.Create("\"", 6877)

#line 173 "..\..\Views\Provider\Detail.cshtml"
, Tuple.Create(Tuple.Create("", 6866), Tuple.Create<System.Object, System.Int32>(classTitle

#line default
#line hidden
, 6866), false)
);

WriteLiteralTo(__razor_helper_writer, ">");


#line 173 "..\..\Views\Provider\Detail.cshtml"
  WriteTo(__razor_helper_writer, title);


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "</dt>\n");

WriteLiteralTo(__razor_helper_writer, "        <dd>\n            <a");

WriteAttributeTo(__razor_helper_writer, "href", Tuple.Create(" href=\"", 6918), Tuple.Create("\"", 6942)

#line 175 "..\..\Views\Provider\Detail.cshtml"
, Tuple.Create(Tuple.Create("", 6925), Tuple.Create<System.Object, System.Int32>(linkProtocol

#line default
#line hidden
, 6925), false)

#line 175 "..\..\Views\Provider\Detail.cshtml"
, Tuple.Create(Tuple.Create("", 6938), Tuple.Create<System.Object, System.Int32>(url

#line default
#line hidden
, 6938), false)
);

WriteLiteralTo(__razor_helper_writer, " rel=\"external\"");

WriteLiteralTo(__razor_helper_writer, " target=\"_blank\"");

WriteAttributeTo(__razor_helper_writer, "class", Tuple.Create(" class=\"", 6974), Tuple.Create("\"", 7008)

#line 175 "..\..\Views\Provider\Detail.cshtml"
, Tuple.Create(Tuple.Create("", 6982), Tuple.Create<System.Object, System.Int32>(cssClass

#line default
#line hidden
, 6982), false)

#line 175 "..\..\Views\Provider\Detail.cshtml"
       , Tuple.Create(Tuple.Create(" ", 6991), Tuple.Create<System.Object, System.Int32>(classIdentifier

#line default
#line hidden
, 6992), false)
);

WriteLiteralTo(__razor_helper_writer, ">\n");


#line 176 "..\..\Views\Provider\Detail.cshtml"
                

#line default
#line hidden

#line 176 "..\..\Views\Provider\Detail.cshtml"
                 if (string.IsNullOrEmpty(urlTitle))
                {
                    

#line default
#line hidden

#line 178 "..\..\Views\Provider\Detail.cshtml"
WriteTo(__razor_helper_writer, url);


#line default
#line hidden

#line 178 "..\..\Views\Provider\Detail.cshtml"
                        
                }
                else
                {
                    

#line default
#line hidden

#line 182 "..\..\Views\Provider\Detail.cshtml"
WriteTo(__razor_helper_writer, urlTitle);


#line default
#line hidden

#line 182 "..\..\Views\Provider\Detail.cshtml"
                             
                }


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "            </a>\n        </dd>\n");


#line 186 "..\..\Views\Provider\Detail.cshtml"
    }


#line default
#line hidden
});

#line 187 "..\..\Views\Provider\Detail.cshtml"
}
#line default
#line hidden

#line 189 "..\..\Views\Provider\Detail.cshtml"
public System.Web.WebPages.HelperResult GetSatisfactionsHtml()
 {
#line default
#line hidden
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {

#line 190 "..\..\Views\Provider\Detail.cshtml"
  


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "     <div");

WriteLiteralTo(__razor_helper_writer, " class=\"rates-list\"");

WriteLiteralTo(__razor_helper_writer, ">\n         <div>\n             <b>Employer satisfaction:</b>\n             <span");

WriteLiteralTo(__razor_helper_writer, " id=\"employer-satisfaction\"");

WriteLiteralTo(__razor_helper_writer, ">\n");

WriteLiteralTo(__razor_helper_writer, "            ");


#line 195 "..\..\Views\Provider\Detail.cshtml"
WriteTo(__razor_helper_writer, Model.EmployerSatisfactionMessage);


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "\n        </span>\n");

WriteLiteralTo(__razor_helper_writer, "             ");


#line 197 "..\..\Views\Provider\Detail.cshtml"
WriteTo(__razor_helper_writer, CreateProgressBar(Model.EmployerSatisfactionMessage, (int)Model.EmployerSatisfaction));


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "\n         </div>\n\n         <div>\n             <b>Learner satisfaction:</b>\n      " +
"       <span");

WriteLiteralTo(__razor_helper_writer, " id=\"learner-satisfaction\"");

WriteLiteralTo(__razor_helper_writer, ">\n");

WriteLiteralTo(__razor_helper_writer, "            ");


#line 203 "..\..\Views\Provider\Detail.cshtml"
WriteTo(__razor_helper_writer, Model.LearnerSatisfactionMessage);


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "\n        </span>\n");

WriteLiteralTo(__razor_helper_writer, "             ");


#line 205 "..\..\Views\Provider\Detail.cshtml"
WriteTo(__razor_helper_writer, CreateProgressBar(Model.LearnerSatisfactionMessage, (int)Model.LearnerSatisfaction));


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "\n        </div>\n\n");


#line 208 "..\..\Views\Provider\Detail.cshtml"
        

#line default
#line hidden

#line 208 "..\..\Views\Provider\Detail.cshtml"
         if (!(Model.EmployerSatisfactionMessage == "no data available" && Model.LearnerSatisfactionMessage == "no data available"))
        {


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "            <p");

WriteLiteralTo(__razor_helper_writer, " class=\"satisfaction-source font-small\"");

WriteLiteralTo(__razor_helper_writer, ">\n                Source: <a");

WriteAttributeTo(__razor_helper_writer, "href", Tuple.Create(" href=\"", 8104), Tuple.Create("\"", 8139)

#line 211 "..\..\Views\Provider\Detail.cshtml"
, Tuple.Create(Tuple.Create("", 8111), Tuple.Create<System.Object, System.Int32>(Model.SatisfactionSourceUrl

#line default
#line hidden
, 8111), false)
);

WriteLiteralTo(__razor_helper_writer, " target=\"_blank\"");

WriteLiteralTo(__razor_helper_writer, ">Skills Funding Agency FE Choices</a>\n            </p>\n");


#line 213 "..\..\Views\Provider\Detail.cshtml"
        }


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "        <hr/>\n\n         <div>\n             <b>Achievement rate:</b>\n             " +
"<span");

WriteLiteralTo(__razor_helper_writer, " class=\"national-rate\"");

WriteLiteralTo(__razor_helper_writer, ">\n");

WriteLiteralTo(__razor_helper_writer, "                 ");


#line 219 "..\..\Views\Provider\Detail.cshtml"
WriteTo(__razor_helper_writer, Model.AchievementRateMessage);


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "\n             </span>\n");


#line 221 "..\..\Views\Provider\Detail.cshtml"
             

#line default
#line hidden

#line 221 "..\..\Views\Provider\Detail.cshtml"
              if (Model.AchievementRateMessage != "no data available")
             {
                 

#line default
#line hidden

#line 223 "..\..\Views\Provider\Detail.cshtml"
WriteTo(__razor_helper_writer, CreateProgressBar(Model.AchievementRateMessage, Model.AchievementRate));


#line default
#line hidden

#line 223 "..\..\Views\Provider\Detail.cshtml"
                                                                                        
             }


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "         </div>\n");


#line 226 "..\..\Views\Provider\Detail.cshtml"
         

#line default
#line hidden

#line 226 "..\..\Views\Provider\Detail.cshtml"
          if (Model.AchievementRateMessage != "no data available")
         {


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "             <div");

WriteLiteralTo(__razor_helper_writer, " class=\"national-achievement-bar\"");

WriteLiteralTo(__razor_helper_writer, ">\n                 <b");

WriteLiteralTo(__razor_helper_writer, " class=\"font-small\"");

WriteLiteralTo(__razor_helper_writer, ">National average:</b>\n                 <span");

WriteLiteralTo(__razor_helper_writer, " class=\"national-rate\"");

WriteLiteralTo(__razor_helper_writer, ">\n");

WriteLiteralTo(__razor_helper_writer, "                     ");


#line 231 "..\..\Views\Provider\Detail.cshtml"
WriteTo(__razor_helper_writer, Model.NationalAchievementRateMessage);


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "\n                 </span>\n");

WriteLiteralTo(__razor_helper_writer, "                 ");


#line 233 "..\..\Views\Provider\Detail.cshtml"
WriteTo(__razor_helper_writer, CreateProgressBar(Model.NationalAchievementRateMessage, Model.NationalAchievementRate));


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "\n             </div>\n");


#line 235 "..\..\Views\Provider\Detail.cshtml"
         }


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "        ");


#line 236 "..\..\Views\Provider\Detail.cshtml"
         if (Model.AchievementRateMessage != "no data available")
        {


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "            <details");

WriteLiteralTo(__razor_helper_writer, " class=\"more-information\"");

WriteLiteralTo(__razor_helper_writer, ">\n                <summary>Explain achievement rate</summary>\n                <di" +
"v");

WriteLiteralTo(__razor_helper_writer, " class=\"panel panel-border-narrow\"");

WriteLiteralTo(__razor_helper_writer, ">\n                    <p>Percentage of apprentices who successfully achieved a si" +
"milar apprenticeship with this training provider.</p>\n                    <p>Bas" +
"ed on the latest data for ");


#line 242 "..\..\Views\Provider\Detail.cshtml"
                      WriteTo(__razor_helper_writer, Model.OverallCohort);


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, " apprentices.</p>\n                    <p");

WriteLiteralTo(__razor_helper_writer, " class=\"font-small\"");

WriteLiteralTo(__razor_helper_writer, ">Source: <a");

WriteAttributeTo(__razor_helper_writer, "href", Tuple.Create(" href=\"", 9576), Tuple.Create("\"", 9614)

#line 243 "..\..\Views\Provider\Detail.cshtml"
, Tuple.Create(Tuple.Create("", 9583), Tuple.Create<System.Object, System.Int32>(Model.AchievementRateSourceUrl

#line default
#line hidden
, 9583), false)
);

WriteLiteralTo(__razor_helper_writer, " target=\"_blank\"");

WriteLiteralTo(__razor_helper_writer, " rel=\"external\"");

WriteLiteralTo(__razor_helper_writer, ">Skills Funding Agency apprenticeship achievement rates</a></p>\n                <" +
"/div>\n            </details>\n");


#line 246 "..\..\Views\Provider\Detail.cshtml"
        }


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "\n    </div>\n");


#line 249 "..\..\Views\Provider\Detail.cshtml"


#line default
#line hidden
});

#line 249 "..\..\Views\Provider\Detail.cshtml"
}
#line default
#line hidden

#line 251 "..\..\Views\Provider\Detail.cshtml"
public System.Web.WebPages.HelperResult CreateProgressBar(string message, int progress)
{
#line default
#line hidden
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {

#line 252 "..\..\Views\Provider\Detail.cshtml"
 
    if (progress > 0)
    {


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "        <div");

WriteLiteralTo(__razor_helper_writer, " class=\"progress-container\"");

WriteLiteralTo(__razor_helper_writer, ">\n            <div");

WriteLiteralTo(__razor_helper_writer, " class=\"progressbar\"");

WriteAttributeTo(__razor_helper_writer, "style", Tuple.Create(" style=\"", 9944), Tuple.Create("\"", 9969)
, Tuple.Create(Tuple.Create("", 9952), Tuple.Create("width:", 9952), true)

#line 256 "..\..\Views\Provider\Detail.cshtml"
, Tuple.Create(Tuple.Create(" ", 9958), Tuple.Create<System.Object, System.Int32>(progress

#line default
#line hidden
, 9959), false)
, Tuple.Create(Tuple.Create("", 9968), Tuple.Create("%", 9968), true)
);

WriteLiteralTo(__razor_helper_writer, "></div>\n        </div>\n");


#line 258 "..\..\Views\Provider\Detail.cshtml"
    }


#line default
#line hidden
});

#line 259 "..\..\Views\Provider\Detail.cshtml"
}
#line default
#line hidden

#line 261 "..\..\Views\Provider\Detail.cshtml"
public System.Web.WebPages.HelperResult GetDeliveryModesHtml(string title, List<string> items, bool hideIfEmpty = false)
{
#line default
#line hidden
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {

#line 262 "..\..\Views\Provider\Detail.cshtml"
 
if (items != null)
{


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "    <dt");

WriteLiteralTo(__razor_helper_writer, " class=\"training-structure\"");

WriteLiteralTo(__razor_helper_writer, ">");


#line 265 "..\..\Views\Provider\Detail.cshtml"
     WriteTo(__razor_helper_writer, title);


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "</dt>\n");

WriteLiteralTo(__razor_helper_writer, "    <dd");

WriteLiteralTo(__razor_helper_writer, " id=\"delivery-modes\"");

WriteLiteralTo(__razor_helper_writer, ">\n        <ul>\n");


#line 268 "..\..\Views\Provider\Detail.cshtml"
            

#line default
#line hidden

#line 268 "..\..\Views\Provider\Detail.cshtml"
             if (items.Exists(m => m.Equals("DayRelease")))
            {


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "                <li");

WriteLiteralTo(__razor_helper_writer, " class=\"day-release\"");

WriteLiteralTo(__razor_helper_writer, ">");


#line 270 "..\..\Views\Provider\Detail.cshtml"
          WriteTo(__razor_helper_writer, Html.Raw("day release"));


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "</li>\n");


#line 271 "..\..\Views\Provider\Detail.cshtml"
            }


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "            ");


#line 272 "..\..\Views\Provider\Detail.cshtml"
             if (items.Exists(m => m.Equals("BlockRelease")))
            {


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "                <li");

WriteLiteralTo(__razor_helper_writer, " class=\"block-release\"");

WriteLiteralTo(__razor_helper_writer, ">");


#line 274 "..\..\Views\Provider\Detail.cshtml"
            WriteTo(__razor_helper_writer, Html.Raw("block release"));


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "</li>\n");


#line 275 "..\..\Views\Provider\Detail.cshtml"
            }


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "            ");


#line 276 "..\..\Views\Provider\Detail.cshtml"
             if (items.Exists(m => m.Equals("100PercentEmployer")))
            {


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "                <li");

WriteLiteralTo(__razor_helper_writer, " class=\"hundred-percent-employer\"");

WriteLiteralTo(__razor_helper_writer, ">");


#line 278 "..\..\Views\Provider\Detail.cshtml"
                       WriteTo(__razor_helper_writer, Html.Raw("at your location"));


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "</li>\n");


#line 279 "..\..\Views\Provider\Detail.cshtml"
            }


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "        </ul>\n        <details>\n            <summary>Explain training options</su" +
"mmary>\n            <div");

WriteLiteralTo(__razor_helper_writer, " class=\"panel panel-border-narrow\"");

WriteLiteralTo(__razor_helper_writer, ">\n                <p>\n                    <span");

WriteLiteralTo(__razor_helper_writer, " class=\"bold-small\"");

WriteLiteralTo(__razor_helper_writer, ">Day release:</span> for example one day a week at the training provider\'s locati" +
"on.\n                </p>\n                <p>\n                    <span");

WriteLiteralTo(__razor_helper_writer, " class=\"bold-small\"");

WriteLiteralTo(__razor_helper_writer, ">Block release:</span> for example 3-4 weeks at the training provider\'s location." +
"\n                </p>\n                <p>\n                    <span");

WriteLiteralTo(__razor_helper_writer, " class=\"bold-small\"");

WriteLiteralTo(__razor_helper_writer, ">At your location:</span> the training provider comes to your workplace.\n        " +
"        </p>\n            </div>\n        </details>\n    </dd>\n");


#line 296 "..\..\Views\Provider\Detail.cshtml"
}


#line default
#line hidden
});

#line 297 "..\..\Views\Provider\Detail.cshtml"
}
#line default
#line hidden

        public Detail()
        {
        }
        public override void Execute()
        {
            
            #line 8 "..\..\Views\Provider\Detail.cshtml"
  
    ViewBag.Title = @Model.Name + " - Apprenticeship Provider";
    ViewBag.Description = "The Find Apprenticeship Training service is for employers in England who want to find training courses for their apprentices and search for training providers.";

            
            #line default
            #line hidden
WriteLiteral("\n<main");

WriteLiteral(" id=\"content\"");

WriteLiteral(" class=\"provider-detail\"");

WriteLiteral(">\n    <div");

WriteLiteral(" class=\"grid-row\"");

WriteLiteral(">\n        <div");

WriteLiteral(" class=\"column-two-thirds\"");

WriteLiteral(">\n            <div>\n                <h1");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(" id=\"provider-name\"");

WriteLiteral(">\n");

WriteLiteral("                    ");

            
            #line 17 "..\..\Views\Provider\Detail.cshtml"
               Write(Model.Name);

            
            #line default
            #line hidden
WriteLiteral("\n                </h1>\n");

            
            #line 19 "..\..\Views\Provider\Detail.cshtml"
                
            
            #line default
            #line hidden
            
            #line 19 "..\..\Views\Provider\Detail.cshtml"
                 if (Is<NationalProvidersFeature>.Enabled && Model.NationalProvider)
                {

            
            #line default
            #line hidden
WriteLiteral("                    <p");

WriteLiteral(" class=\"national-message\"");

WriteLiteral(">\n                        <span");

WriteLiteral(" class=\"tag tag-new\"");

WriteLiteral(">National</span> This training provider is willing to offer apprenticeship traini" +
"ng across England.\n                    </p>\n");

            
            #line 24 "..\..\Views\Provider\Detail.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("\n");

            
            #line 26 "..\..\Views\Provider\Detail.cshtml"
                
            
            #line default
            #line hidden
            
            #line 26 "..\..\Views\Provider\Detail.cshtml"
                 if (Is<FatLevyJourneyFeature>.Enabled)
                {


                    if (Model.IsLevyPayingEmployer.HasValue && Model.IsLevyPayingEmployer.Value && Model.HasNonLevyContract == false)
                     {

            
            #line default
            #line hidden
WriteLiteral("                         <p");

WriteLiteral(" class=\"detail-highlight\"");

WriteLiteral(">\n                             Only levy paying employers can work with this prov" +
"ider\n                         </p>\n");

            
            #line 35 "..\..\Views\Provider\Detail.cshtml"
                     }

                    if (Model.HasParentCompanyGuarantee)
                     {

            
            #line default
            #line hidden
WriteLiteral("                         <p");

WriteLiteral(" class=\"detail-highlight\"");

WriteLiteral(">\n                             Provider is supported by a parent company guarante" +
"e\n                         </p>\n");

            
            #line 42 "..\..\Views\Provider\Detail.cshtml"
                     }

                    if (Model.IsNewProvider)
                     {

            
            #line default
            #line hidden
WriteLiteral("                         <p");

WriteLiteral(" class=\"detail-highlight\"");

WriteLiteral(">\n                             New organisation with no financial track record\n  " +
"                       </p>\n");

            
            #line 49 "..\..\Views\Provider\Detail.cshtml"
                     }
                }

            
            #line default
            #line hidden
WriteLiteral("                \n                <div");

WriteLiteral(" id=\"marketing\"");

WriteLiteral(" class=\"provider-marketing-info\"");

WriteLiteral(">\n");

WriteLiteral("                    ");

            
            #line 53 "..\..\Views\Provider\Detail.cshtml"
               Write(Html.MarkdownToHtml(Model.ProviderMarketingInfo));

            
            #line default
            #line hidden
WriteLiteral("\n                </div>\n            </div>\r\n\r\n            <section>\n             " +
"   <header");

WriteLiteral(" class=\"hgroup\"");

WriteLiteral(">\n                    <h2");

WriteLiteral(" class=\"heading-large apprenticeship-name-level\"");

WriteLiteral(">\n");

WriteLiteral("                        ");

            
            #line 60 "..\..\Views\Provider\Detail.cshtml"
                   Write(Model.ApprenticeshipName);

            
            #line default
            #line hidden
WriteLiteral("\n                    </h2>\n                    <b>Level:</b>\n                    " +
"<span>\n");

WriteLiteral("                        ");

            
            #line 64 "..\..\Views\Provider\Detail.cshtml"
                   Write(Model.ApprenticeshipLevel);

            
            #line default
            #line hidden
WriteLiteral(" (equivalent to ");

            
            #line 64 "..\..\Views\Provider\Detail.cshtml"
                                                             Write(EquivalenceLevelService.GetApprenticeshipLevel(@Model.ApprenticeshipLevel));

            
            #line default
            #line hidden
WriteLiteral(")\n                    </span>\n                </header>\r\n                <dl");

WriteLiteral(" class=\"data-list data-list--compact\"");

WriteLiteral(">\r\n");

WriteLiteral("                    ");

            
            #line 68 "..\..\Views\Provider\Detail.cshtml"
               Write(GetStandardPropertyHtml("Legal Trading Name:", "legal-name", Model.LegalName));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 69 "..\..\Views\Provider\Detail.cshtml"
               Write(GetStandardPropertyAsLinkHtml("Website:", "course-link", "apprenticeshipContactTitle", "apprenticeshipContact", @Model.Apprenticeship.ApprenticeshipInfoUrl, "training provider website"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 70 "..\..\Views\Provider\Detail.cshtml"
               Write(GetStandardPropertyHtml("Phone:", "phone", Model.ContactInformation.Phone));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 71 "..\..\Views\Provider\Detail.cshtml"
               Write(GetEmailPropertyHtml("Email:", "email", Model.ContactInformation.Email));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 72 "..\..\Views\Provider\Detail.cshtml"
               Write(GetStandardPropertyAsLinkHtml("Contact page:", "contact-link", "providerContactTitle", "providerContact", @Model.ContactInformation.ContactUsUrl, "contact this training provider"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 73 "..\..\Views\Provider\Detail.cshtml"
               Write(GetDeliveryModesHtml("Training options:", Model.DeliveryModes));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 74 "..\..\Views\Provider\Detail.cshtml"
               Write(ShowTrainingLocation("Training location:"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </dl>\n\n            </section>\n\n            <section>\n          " +
"      <header>\n                    <h2");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">Apprenticeship training information</h2>\n                </header>\n             " +
"   <article");

WriteLiteral(" class=\"apprenticeship-marketing-info\"");

WriteLiteral(">\n");

WriteLiteral("                    ");

            
            #line 84 "..\..\Views\Provider\Detail.cshtml"
               Write(Html.MarkdownToHtml(Model.Apprenticeship.ApprenticeshipMarketingInfo));

            
            #line default
            #line hidden
WriteLiteral("\n                    <p><em>Content maintained by ");

            
            #line 85 "..\..\Views\Provider\Detail.cshtml"
                                            Write(Model.Name);

            
            #line default
            #line hidden
WriteLiteral("</em>\n                    </p>\n                </article>\n            </section>\n" +
"        </div>\n\n        <div");

WriteLiteral(" class=\"column-third\"");

WriteLiteral(">\n            <div");

WriteLiteral(" class=\"related-container\"");

WriteLiteral(">\n                <aside");

WriteLiteral(" class=\"related\"");

WriteLiteral(">\n                    <h2");

WriteLiteral(" class=\"heading-medium\"");

WriteLiteral(">\n                        Training provider quality assessment\n                  " +
"  </h2>\n");

WriteLiteral("                    ");

            
            #line 97 "..\..\Views\Provider\Detail.cshtml"
               Write(GetSatisfactionsHtml());

            
            #line default
            #line hidden
WriteLiteral("\n                </aside>\n            </div>\n        </div>\n    </div>\n\n    <div");

WriteLiteral(" class=\"grid-row\"");

WriteLiteral(">\n        <div");

WriteLiteral(" class=\"column-two-thirds\"");

WriteLiteral(">\n\n            <div");

WriteLiteral(" class=\"survey-panel\"");

WriteLiteral(">\n                <h2");

WriteLiteral(" class=\"bold-large\"");

WriteLiteral(@">
                    Give us your feedback
                </h2>
                <p>
                    This is a new service and your feedback will help us improve it.<br />
                    Use the link below to take part in a short survey.
                </p>
                <a");

WriteAttribute("href", Tuple.Create(" href=\"", 4937), Tuple.Create("\"", 4960)
            
            #line 114 "..\..\Views\Provider\Detail.cshtml"
, Tuple.Create(Tuple.Create("", 4944), Tuple.Create<System.Object, System.Int32>(Model.SurveyUrl
            
            #line default
            #line hidden
, 4944), false)
);

WriteLiteral(" target=\"_blank\"");

WriteLiteral(" class=\"button\"");

WriteLiteral(">Take the survey</a>\n            </div>\n            <aside");

WriteLiteral(" class=\"disclaimer\"");

WriteLiteral(">\n                <h3");

WriteLiteral(" class=\"heading-small\"");

WriteLiteral(@">Content disclaimer</h3>
                <p>
                    Skills Funding Agency cannot guarantee the accuracy of course information on this site and makes no representations about the quality of any courses which appear on the site. Skills Funding Agency is not liable for any losses suffered as a result of any party relying on the course information provided.
                </p>
            </aside>
        </div>
    </div>

</main>



");

WriteLiteral("\n");

WriteLiteral("\n");

WriteLiteral("\n");

WriteLiteral("\n");

WriteLiteral("\n");

WriteLiteral("\n");

        }
    }
}
#pragma warning restore 1591
