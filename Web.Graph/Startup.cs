using Microsoft.Owin;
using Owin;
using Web.Graph;

[assembly: OwinStartup(typeof(Startup))]
namespace Web.Graph
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
