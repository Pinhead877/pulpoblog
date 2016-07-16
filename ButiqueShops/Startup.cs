using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ButiqueShops.Startup))]
namespace ButiqueShops
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
