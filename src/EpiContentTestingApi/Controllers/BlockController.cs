using System.Web.Http;
using EpiContent.Api.Services;
using EpiContentTestingApi.Services;
using EpiControlTestingApi.Common;
using EPiServer.ServiceLocation;

namespace EpiContent.Api.Controllers
{
    public class BlockController : ApiController
    {
        private readonly IBlockService service;

        public BlockController()
            : this(ServiceLocator.Current.GetInstance<IBlockService>())
        {
        }

        private BlockController(IBlockService blockService)
        {
            service = blockService;
        }

        public BlockDto Add(BlockDto block)
        {
            return service.Add(block);
        }
    }
}