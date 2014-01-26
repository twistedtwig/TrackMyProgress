using System.Linq;
using GoalManagementLibrary;
using GoalManagementLibrary.Models;
using System.Web.Mvc;
using Models;

namespace Web.Controllers
{
    public class CategoriesController : Controller
    {
        private CategoryManager _categoryManager;

        public CategoriesController(CategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        

        public ActionResult Index()
        {
            return View(_categoryManager.Categories());
        }

        public ActionResult CreateCategory()
        {
            return View(new CreateCategoryRequest());
        }


        [HttpPost]
        public ActionResult CreateCategory(CreateCategoryRequest request)
        {
            var result = _categoryManager.CreateCategory(request);
            if (result.Success)
            {
                return RedirectToAction("Index");
            }

            return View("CreateCategoryResult", result);
        }


        public ActionResult Edit(int categoryId)
        {
            var category = _categoryManager.Categories().FirstOrDefault(c => c.Id == categoryId);
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
            var category = _categoryManager.Categories().FirstOrDefault(c => c.Id == categoryId);
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
