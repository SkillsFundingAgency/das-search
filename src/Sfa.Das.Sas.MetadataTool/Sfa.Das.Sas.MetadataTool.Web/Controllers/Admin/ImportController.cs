namespace Sfa.Das.Sas.MetadataTool.Web.Controllers.Admin
{
    using System.Web.Mvc;

    using ApplicationServices.Services.Interfaces;

    public class ImportController : Controller
    {
        private readonly IDocumentImporter _documentImporter;

        public ImportController(IDocumentImporter documentImporter)
        {
            _documentImporter = documentImporter;
        }

        [HttpGet]
        public ActionResult ImportEntries()
        {
            var viewModel = new ImportEntriesViewModel();
            return View(viewModel);
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ImportEntries(string text, string type)
        {
            var viewModel = new ImportEntriesViewModel();

            var message = _documentImporter.Import(text, type);

            viewModel.Message = message;

            return View(viewModel);
        }
    }

    public class ImportEntriesViewModel
    {
        public string Message { get; set; }
    }
}