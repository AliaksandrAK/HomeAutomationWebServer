using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HomeAutomationWebServer.Startup))]
namespace HomeAutomationWebServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
