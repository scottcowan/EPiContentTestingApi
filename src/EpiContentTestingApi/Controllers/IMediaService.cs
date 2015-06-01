using EpiControlTestingApi.Common;
using EPiServer.Core;

namespace EpiContent.Api.Controllers
{
    public interface IMediaService
    {
        ContentReference Add(MediaDto media);
    }
}