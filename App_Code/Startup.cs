using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BaikunthiDeviCMS.Startup))]
namespace BaikunthiDeviCMS
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
