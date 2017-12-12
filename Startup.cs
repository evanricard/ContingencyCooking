using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ContingencyCooking.Startup))]
namespace ContingencyCooking
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
