using Behaviours.Enums;
using GoalManagementLibrary;
using System;
using System.Linq;
using System.Web.Mvc;
using Models;
using Web.Models;

namespace Web.Controllers
{
    public class TrackerController : Controller
    {
        private GoalManager _goalManager;

        public TrackerController(GoalManager goalManager)
        {
            _goalManager = goalManager;
        }


        public ActionResult Index()
        {
            return View(_goalManager.Goals().Where(g => g.StartDate <= DateTime.Now).ToList());
        }

        public ActionResult GoalSelector(DateTime currentDate)
        {
            var model = new GoalSelectorModel { Goals = _goalManager.Goals(currentDate) };

            if (!model.Goals.Any())
            {
                return PartialView("NoGoalsFound");
            }

            return PartialView("GoalSelector", model);
        }

        public ActionResult GetCurrentIteration(int goalId, DateTime selectedDate)
        {
            selectedDate = selectedDate.AddSeconds(1);
            var goal = _goalManager.Get(goalId);

            goal = _goalManager.EnsureGoalHasAllIterations(goal, selectedDate);
            var iteration = _goalManager.GetCurrentIteration(goal, selectedDate);
            if (iteration == null)
            {
                return PartialView("IterationFailedToLoad", new GoalIterationFailedToLoadSummary(goal.Id, goal.Name));
            }

            var iterationSummary = new GoalIterationSummary(iteration, goal.UnitDescription, goal.IntervalDuration.GetDescription());

            return PartialView("IterationSummary", iterationSummary);
        }

        public ActionResult Add(int goalId, DateTime currentDate)
        {
            var goal = _goalManager.Get(goalId);
            if (goal == null) return new EmptyResult();
            if(goal.StartDate > currentDate) return new EmptyResult();


            goal = _goalManager.EnsureGoalHasAllIterations(goal, currentDate);
            var gim = new GoalIterationModel
                {
                    GoalId = goal.Id, 
                    Units = goal.UnitDescription, 
                    IterationId = _goalManager.GetCurrentIteration(goal, currentDate).Id
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
            return Json(_goalManager.AddEntry(iterationModel.GoalId, iterationModel.IterationId, iterationModel.CurrentDate, iterationModel.NewValue));
        }

        public ActionResult IterationEntry(int id)
        {
            var detail = _goalManager.GetIterationDetail(id);
            if (detail == null) return new EmptyResult();

            return PartialView("IterationDetail", detail);
        }

        [HttpPost]
        public JsonResult Delete(IterationDetail detail)
        {
            //todo delete.
            return Json(_goalManager.Delete(detail));
        }
    }
}