using System.Web.Http;

namespace VotingRecord
{
    /// <summary>
    /// WebApplication Start Class
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// WebApplication Start configuration method
        /// </summary>
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}