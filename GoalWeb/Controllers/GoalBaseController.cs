using System.Web.Http;
using IdentityModels;
using IdentityProcess;
using Microsoft.AspNet.Identity;
using RepositoryInterfaces;
using System;
using System.Web.Mvc;

namespace GoalWeb.Controllers
{
    public abstract class GoalBaseController : Controller
    {
        private HnUserStore<ApplicationUser> _userStore;         
        protected UserManager<ApplicationUser> UserManager { get; private set; }

        protected GoalBaseController(IRepo repo)
        {
            _userStore = new HnUserStore<ApplicationUser>(repo);
            UserManager = new UserManager<ApplicationUser>(_userStore);
        }



        private Guid _id = Guid.Empty;
        public Guid UserId 
        { 
            get
            {
                if (_id.Equals(Guid.Empty))
                {
                    var user = UserManager.FindByName(User.Identity.Name);
                    if (user == null)
                    {
                        return _id;
                    }

                    Guid.TryParse(user.Id, out _id);
                }

                return _id;
            } 
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }
    }





    public abstract class GoalBaseApiController : ApiController
    {
        private HnUserStore<ApplicationUser> _userStore;
        protected UserManager<ApplicationUser> UserManager { get; private set; }

        protected GoalBaseApiController(IRepo repo)
        {
            _userStore = new HnUserStore<ApplicationUser>(repo);
            UserManager = new UserManager<ApplicationUser>(_userStore);
        }



        private Guid _id = Guid.Empty;
        public Guid UserId
        {
            get
            {
                if (_id.Equals(Guid.Empty))
                {
                    var user = UserManager.FindByName(User.Identity.Name);
                    if (user == null)
                    {
                        return _id;
                    }

                    Guid.TryParse(user.Id, out _id);
                }

                return _id;
            }
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }
    }
}