using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using GoalManagementLibrary;
using HoHUtilities.Mvc.Windsor;
using NhibernateGoalsRepository;
using Site.Filters;

namespace Site
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            FilterProviders.Providers.Add(new AntiForgeryTokenFilterProvider());

            var container = new WindsorContainer();
            container.Register(AllTypes.FromAssembly(typeof(GoalManager).Assembly).Pick().WithServiceAllInterfaces().Configure(c => c.LifestyleTransient()));
            container.Register(AllTypes.FromAssembly(typeof(GoalRepository).Assembly).Pick().WithServiceAllInterfaces().Configure(c => c.LifestyleTransient()));            
            container.RegisterControllers(Assembly.GetExecutingAssembly());


            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }
    }
}