using Cayci.Helpers;
using Cayci.Entities;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Cayci.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly ProxyHelper _proxyHelper;
        protected IHubContext HubContext { get; private set; }
        public BaseController()
        {
            _proxyHelper = new ProxyHelper();
            Behaviour = JsonRequestBehavior.AllowGet;
            HubContext = GlobalHost.ConnectionManager.GetHubContext<CayciHub>();
        }

        public object Call<TContract, TResult>(Expression<Func<TContract, TResult>> expression) where TContract : IContractBase
        {
            return _proxyHelper.Call(expression);
        }
        protected JsonRequestBehavior Behaviour { get; set; }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var method = ((ReflectedActionDescriptor)filterContext.ActionDescriptor).MethodInfo;
            var isAnonymous = method.CustomAttributes.Any(i => i.AttributeType.Name == typeof(AllowAnonymousAttribute).Name);
            var actionName = filterContext.ActionDescriptor.ActionName;
            if (!isAnonymous)
            {
                if (string.IsNullOrEmpty(SessionHelper.UserId))
                    filterContext.Result = RedirectToAction("Login", "Home");
            }
            if (actionName == "Index" && (SessionHelper.IsOnDuty.HasValue && !SessionHelper.IsOnDuty.Value))
            {
                filterContext.Result = RedirectToAction("NewRequest", "Home");
            }

            base.OnActionExecuting(filterContext);
        }
        protected JsonResult ToJson(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            return Json(json, Behaviour);
        }
    }
}