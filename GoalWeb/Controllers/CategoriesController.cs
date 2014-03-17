using System.Linq;
using System.Web.Mvc;
using Goals.Models;
using Goals.Models.RequestResponse;
using GoalManagement;
using RepositoryInterfaces;

namespace GoalWeb.Controllers
{
    [Authorize]
    public class CategoriesController : GoalBaseController
    {
        private readonly CategoryManager _categoryManager;

        public CategoriesController(CategoryManager categoryManager, IRepo repo) : base(repo)
        {
            _categoryManager = categoryManager;
        }



        public ActionResult Index()
        {
            return View(_categoryManager.Categories(UserId));
        }

        public ActionResult CreateCategory()
        {
            return View(new CreateCategoryRequest());
        }


        [HttpPost]
        public ActionResult CreateCategory(CreateCategoryRequest request)
        {
            request.UserId = UserId;
            var result = _categoryManager.CreateCategory(request);
            if (result.Success)
            {
                return RedirectToAction("Index");
            }

            return View("CreateCategoryResult", result);
        }


        public ActionResult Edit(int categoryId)
        {
            var category = _categoryManager.Categories(UserId).FirstOrDefault(c => c.Id == categoryId);
            if (category == null) return RedirectToAction("Index");

            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (category == null) return RedirectToAction("Index");

            _categoryManager.Save(category);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int categoryId)
        {
            var category = _categoryManager.Categories(UserId).FirstOrDefault(c => c.Id == categoryId);
            if (category == null) return RedirectToAction("Index");

            return View(category);
        }

        [HttpPost]
        public ActionResult Delete(Category category)
        {
            if (category == null) return RedirectToAction("Index");

            _categoryManager.Delete(category);

            return RedirectToAction("Index");
        }
    }
}
