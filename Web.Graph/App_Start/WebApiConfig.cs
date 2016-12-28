using System.Net.Http.Formatting;
using System.Web.Http;

namespace Web.Graph
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de API web
            // Rutas de API web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}"
            );
            //config.Formatters.Clear();
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            //config.Formatters.Add(new FormUrlEncodedMediaTypeFormatter());
            //config.Formatters.Add(new JsonMediaTypeFormatter
            //{
            //    Indent = true
            //});
        }
    }
}
