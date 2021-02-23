using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TiendaWebBicicletas.Startup))]
namespace TiendaWebBicicletas
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
