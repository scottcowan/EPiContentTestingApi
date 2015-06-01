using System.Web.Http;
using System.Web.Routing;
using EpiContentTestingApi;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using StructureMap;

namespace EpiContent.Api
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class ModuleInitialiser : IConfigurableModule
    {
        public void Initialize(InitializationEngine context)
        {
            System.Web.Mvc.ControllerBuilder.Current.DefaultNamespaces.Add("EpiContent.Api.Controllers");
            RouteTables.ConfigureRoutes(RouteTable.Routes);
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        public void Preload(string[] parameters)
        {
        }

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Container.Configure(configureExpression);
        }

        private void configureExpression(ConfigurationExpression obj)
        {
            obj.AddRegistry<ModuleRegistry>();
        }
    }

    public class RouteTables
    {
        public static void ConfigureRoutes(RouteCollection routes)
        {
            routes.MapHttpRoute(
                "EpiContentApi",
                "api/{controller}/{id}",
                new {id = RouteParameter.Optional});
        }
    }
}