using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyElectronicsStore.Startup))]
namespace MyElectronicsStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
