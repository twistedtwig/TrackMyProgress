using GoalManagementLibrary;
using System.Web.Mvc;

namespace Site.Controllers
{
    public class HomeController : Controller
    {
        private GoalManager _goalManager;

        public HomeController(GoalManager goalManager)
        {
            _goalManager = goalManager;
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
