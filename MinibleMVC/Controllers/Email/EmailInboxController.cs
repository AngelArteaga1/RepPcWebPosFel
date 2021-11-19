using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Minible5.Controllers.Email
{
    public class EmailInboxController : Controller
    {
        // GET: EmailInbox
        public ActionResult Index()
        {
            return View();
        }
    }
}