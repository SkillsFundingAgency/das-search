using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using StandardModule;
using StandardModule.Web.UI.Standards;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("StandardModule")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("StandardModule")]
[assembly: AssemblyCopyright("Copyright ©  2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Registers StandardModuleInstaller.PreApplicationStart() to be executed prior to the application start
[assembly: PreApplicationStartMethod(typeof(StandardModuleInstaller), "PreApplicationStart")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("153a7b0d-059a-44e4-b119-9bee33cbb233")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[assembly: WebResource(StandardsPage.StandardsPageScript, "application/x-javascript")]
[assembly: WebResource(StandardsPage.StandardsMasterScript, "application/x-javascript")]
[assembly: WebResource(StandardsPage.StandardsDetailScript, "application/x-javascript")]

[assembly: WebResource("StandardModule.Web.Resources.CustomStylesKendoUIView.css", "text/css", PerformSubstitution = true)]
[assembly: WebResource("StandardModule.Web.Resources.paging.png", "image/gif")]
