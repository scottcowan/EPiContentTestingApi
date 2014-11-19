using EpiContentTestingApi.Services;
using StructureMap.Configuration.DSL;

namespace EpiContentTestingApi
{
    public class ModuleRegistry : Registry
    {
        public ModuleRegistry()
        {
            For<IPageService>().Use<PageService>();
            For<IBlockService>().Use<BlockService>();
        }
    }
}