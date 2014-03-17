using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GoalManagement;
using IdentityModels;
using Microsoft.AspNet.Identity;

namespace GoalWeb.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
//        private UserManager<IdentityUser> _userManager;
//        private NhibernateUserStore<IdentityUser> _userStore;
//        private GoalManager _goalManager;
//
//        public AdminController(IUserRepository userRepository, GoalManager goalManager)
//        {
//            _userStore = new NhibernateUserStore<IdentityUser>(userRepository);
//            _userManager = new UserManager<IdentityUser>(_userStore);
//            _goalManager = goalManager;
//        }
//
        public ActionResult Index()
        {
            return View();
        }
//
//        [HttpPost]
//        public async Task<ActionResult> CreateResetUser()
//        {
//            var name = "testuser";
//            var userbyName = await _userManager.FindByNameAsync(name);
//
//            var testUserSetup = new TestUserSetup(_goalManager);
//
//            if (userbyName != null)
//            {
//                testUserSetup.RemoveAllGoals(Guid.Parse(userbyName.Id));
//                await _userStore.DeleteAsync(userbyName);
//            }
//
//            var user = new IdentityUser { UserName = name };
//            var result = await _userManager.CreateAsync(user, name);
//            testUserSetup.SetupNewGoals(Guid.Parse(userbyName.Id));
//
//            return RedirectToAction("Index", "admin");
//        }

    }
}