using System;
using System.Web.Mvc;

namespace GoalWeb.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult General(Exception exception)
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult Forbidden()
        {
            return View();
        }
	}
}