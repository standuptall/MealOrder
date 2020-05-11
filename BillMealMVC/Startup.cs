using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BillMealMVC.Startup))]
namespace BillMealMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
