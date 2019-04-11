using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.Shared.Components.Web.Models;

namespace Sfa.Das.Sas.Shared.Components.Web.Controllers
{
    public class ModelsController : Controller
    {

        public IActionResult SearchQueryViewModel()
        {
            return View();
        }


    }
}
