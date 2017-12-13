using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(U_News.Startup))]
namespace U_News
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
