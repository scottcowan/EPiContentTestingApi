using EPiServer.Core;

namespace EpiContentTestingApi.Services
{
    public interface IPageFinder
    {
        PageReference Find(string name);
    }
}