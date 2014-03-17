using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GoalWeb.Startup))]
namespace GoalWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
