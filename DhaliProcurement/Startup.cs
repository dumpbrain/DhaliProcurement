using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DhaliProcurement.Startup))]
namespace DhaliProcurement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
