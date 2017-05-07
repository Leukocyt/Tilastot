using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(stateMonitor.Startup))]
namespace stateMonitor
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
