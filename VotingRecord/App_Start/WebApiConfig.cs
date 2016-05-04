using System.Web.Http;

namespace VotingRecord
{
    /// <summary>
    /// Web API Configuration
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Enables dependency injection for controllers
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // attribute based routing
            config.MapHttpAttributeRoutes();

            // convention based routing
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{name}",
                defaults: new { name = RouteParameter.Optional }
            );
        }
    }
}