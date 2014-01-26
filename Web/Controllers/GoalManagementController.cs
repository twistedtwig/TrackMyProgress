using System;
using System.Linq;
using GoalManagementLibrary;
using GoalManagementLibrary.Models;
using System.Web.Mvc;
using Models;
using Web.Models;

namespace Web.Controllers
{
    public class GoalManagementController : Controller
    {
        private GoalManager _goalManager;
        private CategoryManager _categoryManager;

        public GoalManagementController(GoalManager goalManager, CategoryManager categoryManager)
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

            if (result.Success)
            {
                return RedirectToAction("Index");
            }

            return View("CreateGoalResult", result);
        }

        public ActionResult Edit(int goalId)
        {
            var goal = _goalManager.Goals().FirstOrDefault(x => x.Id.Equals(goalId));
            if (goal == null) { return RedirectToAction("Index"); }

            return View(new GoalViewModel(goal, _categoryManager.Categories()));
        }

        [HttpPost]
        public ActionResult Edit(GoalViewModel goal)
        {
            if (goal == null) return RedirectToAction("Index");

            var g = goal.RecreateGoal();
            g.Intervals = _goalManager.Goals().First(x => x.Id.Equals(g.Id)).Intervals;

            var result = _goalManager.Save(g);
            if (!result.Success)
            {
                result.Model = new GoalViewModel(result.Model, _categoryManager.Categories());
                return View("EditFailed", result);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int goalId)
        {
            var goal = _goalManager.Goals().FirstOrDefault(x => x.Id.Equals(goalId));
            if (goal == null)
            {
                return RedirectToAction("Index");
            }

            return View(goal);
        }


        [HttpPost]
        public ActionResult Delete(Goal goal)
        {
            if (goal == null) { return RedirectToAction("Index"); }

            _goalManager.Delete(goal);
            return RedirectToAction("Index");
        }
    }
}
