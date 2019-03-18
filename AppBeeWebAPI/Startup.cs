using AppBeeWebAPI.Board;
using Microsoft.Owin.Cors;
using Owin;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using Unity;
using Unity.Lifetime;

namespace AppBeeWebAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseWelcomePage("/");
            app.UseCors(CorsOptions.AllowAll);
            

            var config = new HttpConfiguration();

            config.EnableCors(new EnableCorsAttribute("http://localhost:8080", "*", "*"));

            config.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
            config.Formatters.JsonFormatter.SerializerSettings.DateFormatString = "dd/MM/yyyy HH:mm:ss";

            var jsonFormatter = new JsonMediaTypeFormatter();
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter));


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var container = new UnityContainer();
            container.RegisterType<ActivityBoard, ActivityBoard>(new ContainerControlledLifetimeManager());
            container.RegisterType<ProjectBoard, ProjectBoard>(new ContainerControlledLifetimeManager());
            container.RegisterType<TaskBoard, TaskBoard>(new ContainerControlledLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);

            
            app.UseWebApi(config);
        }
    }
}
