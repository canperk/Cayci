using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(Cayci.Startup))]
namespace Cayci
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}