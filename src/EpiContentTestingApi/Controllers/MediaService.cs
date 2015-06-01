using System;
using System.IO;
using System.Linq;
using EpiControlTestingApi.Common;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Framework.Blobs;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Web;

namespace EpiContent.Api.Controllers
{
    public class MediaService : IMediaService
    {
        private readonly IContentRepository contentRepository;
        private readonly BlobFactory blobFactory;
        private readonly IContentLoader contentLoader;

        public MediaService() : this(
            ServiceLocator.Current.GetInstance<IContentRepository>(),
            ServiceLocator.Current.GetInstance<BlobFactory>(),
            ServiceLocator.Current.GetInstance<IContentLoader>())
        {
        }

        public MediaService(IContentRepository contentRepository, BlobFactory blobFactory, IContentLoader contentLoader)
        {
            this.contentRepository = contentRepository;
            this.blobFactory = blobFactory;
            this.contentLoader = contentLoader;
        }

        public ContentReference Add(MediaDto media)
        {
            var container = CreateMediaFolder(media.CmsPath);
            if (this.contentRepository.GetChildren<MediaData>(container).Any(x => x.Name == media.FileName))
            {
                return this.contentRepository.GetChildren<MediaData>(container).First(x => x.Name == media.FileName).ContentLink;
            }

            var file = this.contentRepository.GetDefault<MediaData>(container);
            file.Name = media.FileName;

            var extension = media.FileName.Split('.');
            var blob = blobFactory.CreateBlob(file.BinaryDataContainer, "." + extension[extension.Length - 1]);
            var ms = new MemoryStream(media.Data);
            blob.Write(ms);
            file.BinaryData = blob;
            return this.contentRepository.Save(file, SaveAction.Publish, AccessLevel.NoAccess);
        }

        private ContentReference CreateMediaFolder(string cmsPath, ContentReference parent = null)
        {
            ContentReference reference;

            if (parent == null)
            {
                var folderName = cmsPath.Split('/')[0];
                if (folderName == "Global Assets")
                {
                    return CreateMediaFolder(cmsPath.Replace(folderName + "/", string.Empty),
                        SiteDefinition.Current.GlobalAssetsRoot);
                }

                throw new ApplicationException("Path needs to start with Global Assets/");
            }

            if (string.IsNullOrEmpty(cmsPath))
            {
                return parent;
            }

            var name = cmsPath.Split('/')[0];

            if (this.contentLoader.GetChildren<ContentFolder>(parent).Any(x => x.Name == name))
            {
                var contentFolder = this.contentLoader.GetChildren<ContentFolder>(parent).First(x => x.Name == name);
                reference = contentFolder.ContentLink;
            }
            else
            {
                var folder = this.contentRepository.GetDefault<ContentFolder>(parent);
                folder.Name = name;
                reference = this.contentRepository.Save(folder, SaveAction.Publish, AccessLevel.NoAccess);
            }

            var remaining = cmsPath.Replace(name, string.Empty);
            if (remaining.StartsWith("/"))
            {
                remaining = remaining.Substring(1);
            }

            return this.CreateMediaFolder(remaining, reference);
        }
    }
}