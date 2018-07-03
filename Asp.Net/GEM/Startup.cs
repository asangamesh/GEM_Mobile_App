using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GEM.Startup))]
namespace GEM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
