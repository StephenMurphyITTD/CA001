using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CA001.Startup))]
namespace CA001
{
    public partial class Startup
    {
        public void Config(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
