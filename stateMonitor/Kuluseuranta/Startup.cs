using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Kuluseuranta.Startup))]
namespace Kuluseuranta
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
