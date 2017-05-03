using Basbakanlik.Strateji.Cayci.Helpers;
using Basbakanlik.Strateji.Entities;
using Basbakanlik.Strateji.Entities.Contracts;
using Basbakanlik.Strateji.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Basbakanlik.Strateji.Cayci.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewRequest()
        {
            return View();
        }

        public ActionResult MyRequests()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string ddlUser, string ddlLocation)
        {
            if (!string.IsNullOrEmpty(ddlUser))
            {
                var result = Call((IUserContract c) => c.GetUserById(ddlUser)) as ApiResult<User>;
                if(result == null)
                    return RedirectToAction("Index", "Home");
                SessionHelper.UserId = result.Result.ID;
                SessionHelper.DisplayName = result.Result.DisplayName;
                SessionHelper.IsOnDuty = result.Result.IsOnDuty;
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOut()
        {
            SessionHelper.Clear();
            return RedirectToAction("Login", "Home");
        }
    }
}