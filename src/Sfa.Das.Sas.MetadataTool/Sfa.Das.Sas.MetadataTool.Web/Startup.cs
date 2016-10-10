using Microsoft.Owin;

using Sfa.Das.Sas.MetadataTool.Web;

[assembly: OwinStartup(typeof(Startup))]

namespace Sfa.Das.Sas.MetadataTool.Web
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
