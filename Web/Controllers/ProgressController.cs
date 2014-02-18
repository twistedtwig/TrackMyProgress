using GoalManagementLibrary;
using System.Linq;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class ProgressController : Controller
    {
        private GoalManager _goalManager;

        public ProgressController(GoalManager goalManager)
        {
            _goalManager = goalManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GoalSelector()
        {
            var model = new GoalSelectorModel { Goals = _goalManager.Goals() };

            if (!model.Goals.Any())
            {
                return PartialView("NoGoalsFound");
            }

            return PartialView("_GoalSelector", model);
        }

        public ActionResult GoalSummary(int goalId)
        {
            var goalSummary = _goalManager.GetGoalSummary(goalId);
            return PartialView("_GoalSummary", goalSummary);
        }

        public ActionResult GetIterationRangeSelector()
        {
            return PartialView("_IterationRangeSelector");
        }

        public ActionResult MultiIterationSummary(int goalId, int[] iterationIds)
        {
            var summary = _goalManager.GetIterationSummaries(goalId, iterationIds);
            return PartialView("_MultiIterationSummary", summary);
        }
	}
}