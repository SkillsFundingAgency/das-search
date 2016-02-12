# SFA DAS Web

## Projects

### Sfa.Eds.Das.Web
 - ViewModels
 - Controllers
 - Views
 
### Sfa.Eds.Das.Web.Tests
 - NUnit
 - Moq

### Sfa.Eds.Das.Core
 - Models
 - Extensions
 - Search (Infrastructure)
   
### Sfa.Eds.Das.Core.Tests
 - NUnit
 - Moq
 - Razor Generator
  - Razor Generator nuget needs to be installed on web project
  - Razor Generator VS extension need to be installed [Razor Generator](https://visualstudiogallery.msdn.microsoft.com/1f6ec6ff-e89b-4c47-8e79-d2d68df894ec)
  - Razor Generator Test nuget needs to be installed on test project. 
  - Razor Generator config file (razorgenerator.directives) in view folder
  - For View we need to pre compile set **RazorGenerator** in **Custom Tool** on cshtml settings.  
   

###Exclude Test
Decorate nunit tests with ` [Category("Nightly")] ` to exclude them from CI