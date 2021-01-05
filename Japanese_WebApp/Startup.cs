using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Japanese_WebApp.Startup))]
namespace Japanese_WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
