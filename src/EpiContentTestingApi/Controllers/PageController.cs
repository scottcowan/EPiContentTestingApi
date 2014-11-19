using System.Web.Http;
using System.Web.Http.Results;
using EpiContentTestingApi.Services;
using EpiControlTestingApi.Common;
using EPiServer.ServiceLocation;

namespace EpiContent.Api.Controllers
{
    public class PageController : ApiController
    {
        private IPageService service;

        public PageController()
        {
            service = ServiceLocator.Current.GetInstance<IPageService>();
        }

        public OkNegotiatedContentResult<PageDto> Add(PageDto page)
        {
            var result = service.Add(page);
            return Ok(result);
        }
    }
}