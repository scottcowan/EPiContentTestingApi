using System.Web.Http;
using EpiContentTestingApi.Services;
using EpiControlTestingApi.Common;
using EPiServer.ServiceLocation;

namespace EpiContentTestingApi.Controllers
{
    public class PageController : ApiController
    {
        private IPageService service;

        public PageController()
        {
            service = ServiceLocator.Current.GetInstance<IPageService>();
        }

        public PageDto Add(PageDto page)
        {
            return service.Add(page);
        }
    }
}