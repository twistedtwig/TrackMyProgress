﻿using System;
using System.IO;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using GoalManagement;
using GoalRepository;
using GoalWeb.Controllers;
using HoHUtilities.Mvc.Windsor;
using Mvc.Windsor;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

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

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            var httpException = exception as HttpException;
            Response.Clear();
            Server.ClearError();

            WriteToLog(httpException);

            var routeData = new RouteData();
            routeData.Values["controller"] = "Errors";
            routeData.Values["action"] = "General";
            routeData.Values["exception"] = exception;
            Response.StatusCode = 500;
            if (httpException != null)
            {
                Response.StatusCode = httpException.GetHttpCode();
                switch (Response.StatusCode)
                {
                    case 403:
                        routeData.Values["action"] = "Forbidden";
                        break;
                    case 404:
                        routeData.Values["action"] = "NotFound";
                        break;
                }
            }

            IController errorsController = new ErrorsController();
            var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
            errorsController.Execute(rc);               
        }

        private void WriteToLog(Exception ex)
        {
            var builder = new StringBuilder();
            builder.AppendLine(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
            builder.AppendLine(ex.Message);
            builder.AppendLine(ex.StackTrace);
            builder.AppendLine(Environment.NewLine);

            WriteToLog(builder.ToString());
        }

        private void WriteToLog(string message)
        {
            string path = HttpContext.Current.Server.MapPath("~/log.txt");
            File.AppendAllText(path, message);

        }
    }
}
