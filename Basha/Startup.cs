using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Basha.Startup))]
namespace Basha
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
