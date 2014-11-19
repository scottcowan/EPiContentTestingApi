using EpiContentTestingApi;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using StructureMap;

namespace EpiContent.Api
{
    public class ModuleInitialiser : IConfigurableModule
    {
        public void Initialize(InitializationEngine context)
        {
            System.Web.Mvc.ControllerBuilder.Current.DefaultNamespaces.Add("EpiContent.Api.Controllers");
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
}