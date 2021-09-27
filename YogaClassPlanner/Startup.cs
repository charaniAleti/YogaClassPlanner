using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(YogaClassPlanner.Startup))]
namespace YogaClassPlanner
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
