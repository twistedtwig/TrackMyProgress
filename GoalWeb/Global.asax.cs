using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using GoalManagement;
using GoalRepository;
using HoHUtilities.Mvc.Windsor;
using Mvc.Windsor;

namespace GoalWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);




            var container = new WindsorContainer();
            container.Register(AllTypes.FromAssembly(typeof(GoalManager).Assembly).Pick().WithServiceAllInterfaces().Configure(c => c.LifestyleTransient()));
            container.Register(AllTypes.FromAssembly(typeof(DbRepo).Assembly).Pick().WithServiceAllInterfaces().Configure(c => c.LifestyleTransient()));
            container.RegisterControllers(Assembly.GetExecutingAssembly());

            GlobalConfiguration.Configuration.DependencyResolver = new WindsorDependencyResolver(container);
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));
        }
    }
}
