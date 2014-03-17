using GoalManagement;
using GoalWeb.Models;
using System.Linq;
using System.Web.Mvc;
using RepositoryInterfaces;

namespace GoalWeb.Controllers
{
    [Authorize]
    public class ProgressController : GoalBaseController
    {
        private readonly GoalManager _goalManager;

        public ProgressController(GoalManager goalManager, IRepo repo) : base(repo)
        {
            _goalManager = goalManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GoalSelector()
        {
            var model = new GoalSelectorModel { Goals = _goalManager.Goals(UserId) };

            if (!model.Goals.Any())
            {
                return PartialView("NoGoalsFound");
            }

            return PartialView("_GoalSelector", model);
        }

        public ActionResult GoalSummary(int goalId)
        {
            var goalSummary = _goalManager.GetGoalSummary(UserId, goalId);
            return PartialView("_GoalSummary", goalSummary);
        }

        public ActionResult GetIterationRangeSelector()
        {
            return PartialView("_IterationRangeSelector");
        }

        public ActionResult MultiIterationSummary(int goalId, int[] iterationIds)
        {
            var summary = _goalManager.GetIterationSummaries(UserId, goalId, iterationIds);
            return PartialView("_MultiIterationSummary", summary);
        }
    }
}