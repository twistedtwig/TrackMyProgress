using System.Linq;
using GoalManagementLibrary;
using GoalManagementLibrary.Models;
using System.Web.Mvc;

namespace Site.Controllers
{
    public class GoalsController : Controller
    {
        private GoalManager _goalManager;
        private CategoryManager _categoryManager;

        public GoalsController(GoalManager goalManager, CategoryManager categoryManager)
        {
            _goalManager = goalManager;
            _categoryManager = categoryManager;
        }

        

        public ActionResult Index()
        {
            return View(_goalManager.Goals());
        }

        public ActionResult CreateGoal()
        {
            var viewModel = new CreateGoalRequest(_categoryManager.Categories());
            if (viewModel.Categories == null || !viewModel.Categories.Any())
            {
                return View("NeedCategories");
            }

            return View(viewModel);
        }


        [HttpPost]
        public ActionResult CreateGoal(CreateGoalRequest request)
        {
            var result = _goalManager.CreateGoal(request);
            return View("CreateGoalResult", result);
        }


    }
}
