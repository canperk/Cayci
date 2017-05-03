using System.Web;
using System.Web.Mvc;

namespace Basbakanlik.Strateji.Cayci
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
