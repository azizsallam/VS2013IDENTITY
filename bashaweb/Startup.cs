using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(bashaweb.Startup))]
namespace bashaweb
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
