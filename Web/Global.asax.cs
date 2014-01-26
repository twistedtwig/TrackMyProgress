using System.Web.Http;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using GoalManagementLibrary;
using HoHUtilities.Mvc.Windsor;
using Mvc.Windsor;
using NhibernateGoalsRepository;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Web.App_Start;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var container = new WindsorContainer();
            container.Register(AllTypes.FromAssembly(typeof(GoalManager).Assembly).Pick().WithServiceAllInterfaces().Configure(c => c.LifestyleTransient()));
            container.Register(AllTypes.FromAssembly(typeof(GoalRepository).Assembly).Pick().WithServiceAllInterfaces().Configure(c => c.LifestyleTransient()));
            container.RegisterControllers(Assembly.GetExecutingAssembly());
            
            GlobalConfiguration.Configuration.DependencyResolver = new WindsorDependencyResolver(container);
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));
        }
    }
}
