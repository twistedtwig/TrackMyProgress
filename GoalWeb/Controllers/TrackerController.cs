using Goals.Models;
using Goals.Shared.Enums;
using GoalManagement;
using System;
using System.Linq;
using System.Web.Mvc;
using GoalWeb.Models;
using RepositoryInterfaces;

namespace GoalWeb.Controllers
{
    [Authorize]
    public class TrackerController : GoalBaseController
    {
        private readonly GoalManager _goalManager;

        public TrackerController(GoalManager goalManager, IRepo repo) : base(repo)
        {
            _goalManager = goalManager;
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GoalSelector(DateTime currentDate)
        {
            var model = new GoalSelectorModel { Goals = _goalManager.Goals(UserId, currentDate) };

            if (!model.Goals.Any())
            {
                return PartialView("NoGoalsFound");
            }

            return PartialView("_GoalSelector", model);
        }

        public ActionResult GetCurrentIteration(int goalId, DateTime selectedDate)
        {
            selectedDate = selectedDate.AddSeconds(1);
            var goal = _goalManager.Get(UserId, goalId);

            goal = GoalUtilities.EnsureGoalHasAllIterations(goal, selectedDate);
            var iteration = GoalUtilities.GetCurrentIteration(goal, selectedDate);
            if (iteration == null)
            {
                return PartialView("IterationFailedToLoad", new GoalIterationFailedToLoadSummary(goal.Id, goal.Name));
            }

            var iterationSummary = new GoalIterationSummary(iteration, goal.UnitDescription, goal.IntervalDuration.GetDescription());

            return PartialView("IterationSummary", iterationSummary);
        }

        public ActionResult Add(int goalId, DateTime currentDate)
        {
            var goal = _goalManager.Get(UserId, goalId);
            if (goal == null) return new EmptyResult();
            if (goal.StartDate > currentDate) return new EmptyResult();


            goal = GoalUtilities.EnsureGoalHasAllIterations(goal, currentDate);
            var gim = new GoalIterationModel
            {
                GoalId = goal.Id,
                Units = goal.UnitDescription,
                IterationId = GoalUtilities.GetCurrentIteration(goal, currentDate).Id
            };

            switch (goal.GoalType)
            {
                case GoalType.ChangeSomething:
                    return PartialView("IterationAdditionChange", gim);

                case GoalType.ReachSomething:
                    return PartialView("IterationAdditionReach", gim);

                case GoalType.TrackSomething:
                    return PartialView("IterationAddition", gim);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpPost]
        public JsonResult Add(GoalIterationModel iterationModel)
        {
            return Json(_goalManager.AddEntry(UserId, iterationModel.GoalId, iterationModel.IterationId, iterationModel.CurrentDate, iterationModel.NewValue));
        }

        public ActionResult IterationEntry(int id)
        {
            var detail = _goalManager.GetIterationDetail(UserId, id);
            if (detail == null) return new EmptyResult();

            return PartialView("IterationDetail", detail);
        }

        [HttpPost]
        public JsonResult Delete(IterationDetail detail)
        {
            //todo delete.
            return Json(_goalManager.Delete(UserId, detail));
        }
    }
}