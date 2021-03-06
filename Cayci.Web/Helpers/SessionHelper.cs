﻿using System;
using System.Web;

namespace Cayci.Helpers
{
    public static class SessionHelper
    {
        public static string UserId
        {
            get
            {
                if (HttpContext.Current.Session["UserId"] != null)
                    return HttpContext.Current.Session["UserId"].ToString();
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session["UserId"] = value;
            }
        }
        public static string DisplayName
        {
            get
            {
                if (HttpContext.Current.Session["DisplayName"] != null)
                    return HttpContext.Current.Session["DisplayName"].ToString();
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session["DisplayName"] = value;
            }
        }

        public static string Group
        {
            get
            {
                if (HttpContext.Current.Session["Group"] != null)
                    return HttpContext.Current.Session["Group"].ToString();
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session["Group"] = value;
            }
        }

        public static bool? IsOnDuty
        {
            get
            {
                if (HttpContext.Current.Session["IsOnDuty"] != null)
                    return Convert.ToBoolean(HttpContext.Current.Session["IsOnDuty"]);
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session["IsOnDuty"] = value;
            }
        }

        public static void Clear()
        {
            UserId = null;
            DisplayName = null;
            IsOnDuty = null;
            Group = null;
        }
    }
}