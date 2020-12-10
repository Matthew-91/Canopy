using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Canopy.Startup))]
namespace Canopy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
