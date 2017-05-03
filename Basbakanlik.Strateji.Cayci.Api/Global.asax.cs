using Basbakanlik.Strateji.Provider.Context;
using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Basbakanlik.Strateji.Cayci.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            MongoContext.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            MongoContext.DatabaseName = ConfigurationManager.AppSettings["DatabaseName"];
        }
    }
}
