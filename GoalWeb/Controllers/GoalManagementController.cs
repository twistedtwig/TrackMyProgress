using System;
using System.Linq;
using System.Web.Mvc;
using Goals.Models;
using Goals.Models.RequestResponse;
using GoalManagement;
using GoalWeb.Models;
using RepositoryInterfaces;

namespace GoalWeb.Controllers
{
    [Authorize]
    public class GoalManagementController : GoalBaseController
    {
        private readonly GoalManager _goalManager;
        private readonly CategoryManager _categoryManager;

        public GoalManagementController(GoalManager goalManager, CategoryManager categoryManager, IRepo repo) : base(repo)
        {
            _goalManager = goalManager;
            _categoryManager = categoryManager;
        }



        public ActionResult Index()
        {
            return View(_goalManager.Goals(UserId));
        }

        public ActionResult CreateGoal()
        {
            var viewModel = new CreateGoalRequest(_categoryManager.Categories(UserId));
            if (viewModel.Categories == null || !viewModel.Categories.Any())
            {
                return View("NeedCategories");
            }

            return View(viewModel);
        }


        [HttpPost]
        public ActionResult CreateGoal(CreateGoalRequest request)
        {
            request.UserId = UserId;
            var result = _goalManager.CreateGoal(request);

            if (result.Success)
            {
                return RedirectToAction("Index");
            }

            return View("CreateGoalResult", result);
        }

        public ActionResult CreateFromTarget()
        {
            var model = new CreateGoalFromTarget();

            model.Categories = _categoryManager.Categories(UserId);
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateFromTarget(CreateGoalFromTarget model)
        {
            model.UserId = UserId;
            var result = _goalManager.CreateGoalFromTarget(model);

            if (result.Success)
            {
                return RedirectToAction("Index");
            }

            foreach (var message in result.Messages)
            {
                ModelState.AddModelError("", message);
            }

            return View(model);
        }

        public ActionResult Edit(int goalId)
        {
            var goal = _goalManager.Goals(UserId).FirstOrDefault(x => x.Id.Equals(goalId));
            if (goal == null) { return RedirectToAction("Index"); }

            return View(new GoalViewModel(goal, _categoryManager.Categories(UserId)));
        }

        [HttpPost]
        public ActionResult Edit(GoalViewModel goal)
        {
            if (goal == null) return RedirectToAction("Index");

            var g = goal.RecreateGoal();
            g.Intervals = _goalManager.Goals(UserId).First(x => x.Id.Equals(g.Id)).Intervals;

            var result = _goalManager.Save(g);
            if (!result.Success)
            {
                result.Model = new GoalViewModel(result.Model, _categoryManager.Categories(UserId));
                return View("EditFailed", result);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int goalId)
        {
            var goal = _goalManager.Goals(UserId).FirstOrDefault(x => x.Id.Equals(goalId));
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

            _goalManager.Delete(UserId, goal);
            return RedirectToAction("Index");
        }
    }
}
