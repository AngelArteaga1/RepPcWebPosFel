using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Minible5.Controllers.Auth;
using Minible5.Models;

namespace Minible5.Filters
{
    public class VerificaSession : ActionFilterAttribute
    {
        private security_users oUsuario;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                base.OnActionExecuting(filterContext);
                oUsuario = (security_users)HttpContext.Current.Session["User"];
                if(oUsuario == null)
                {
                    if(filterContext.Controller is AuthLoginController == false)
                    {
                        filterContext.HttpContext.Response.Redirect("~/AuthLogin");
                    }
                }
            }
            catch (Exception e)
            {
                filterContext.Result = new RedirectResult("~/AuthLogin");
            }
        }
    }
}