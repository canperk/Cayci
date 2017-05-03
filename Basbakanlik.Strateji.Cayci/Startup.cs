using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(Basbakanlik.Strateji.Cayci.Startup))]
namespace Basbakanlik.Strateji.Cayci
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}