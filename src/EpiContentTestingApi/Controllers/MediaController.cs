using System.Web.Http;
using EpiControlTestingApi.Common;
using EPiServer.ServiceLocation;

namespace EpiContent.Api.Controllers
{
    public class MediaController : ApiController
    {
        private IMediaService service;

        public MediaController() : this(ServiceLocator.Current.GetInstance<IMediaService>())
        {
        }

        private MediaController(IMediaService mediaService)
        {
            service = mediaService;
        }

        public MediaDto Add(MediaDto media)
        {
            return service.Add(media);
        }
    }
}