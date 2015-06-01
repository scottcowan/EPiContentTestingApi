using System;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using EPiServer.Web;

namespace EpiContentTestingApi.Services
{
    public class PageLookup
    {
        public const string PageTitle = "Automated Test Pages";
        private readonly IContentRepository contentRepository;
        private PageReference rootReference;
        private PageReference homePageReference;
        
        public PageLookup(IContentRepository contentRepository)
        {
            this.contentRepository = contentRepository;
        }

        public int HomePageId
        {
            get { return this.homePageReference.ID; }
        }

        public PageReference HomePageReference
        {
            get
            {
                if (this.homePageReference == null)
                {
                    this.homePageReference = this.FindContainer(SiteDefinition.Current.StartPage.ToPageReference(),
                        PageTitle, null, "automated");
                }
                return this.homePageReference;
            }
        }

        public PageReference RootReference
        {
            get
            {
                return this.rootReference ?? (this.rootReference = SiteDefinition.Current.RootPage.ToPageReference());
            }
        }

        public PageData GetExistingPage(string pageName, PageReference pageRef)
        {
            if (string.IsNullOrEmpty(pageName))
            {
                throw new Exception();
            }

            var names = pageName.Split(new char[] {'/'}, 2);
            var page = contentRepository.GetChildren<PageData>(pageRef).FirstOrDefault(x => x.Name == names.First());

            if (page == null)
            {
                return null;
            }

            if (names.Length == 1)
            {
                return page;
            }

            return this.GetExistingPage(names.LastOrDefault(), page.PageLink);
        }

        public PageReference FindContainer(PageReference parent, string pageTitle, string pageType,
            string url = null)
        {
            var page = contentRepository.GetChildren<PageData>(parent).FirstOrDefault(x => x.Name == pageTitle);
            if (page != null)
            {
                return page.PageLink;
            }

            var myType = Type.GetType(pageType);
            var method = typeof (IContentRepository).GetMethod("GetDefault", new[] {typeof (ContentReference)});
            var generic = method.MakeGenericMethod(myType);

            var pageObject = generic.Invoke(this.contentRepository, new[] {parent});
            var castingMethod = typeof (CastingHelper).GetMethod("CastToMyType").MakeGenericMethod(myType);
            var uncast = castingMethod.Invoke(new CastingHelper(), new[] {pageObject});
            var pageBase = uncast as PageData;
            pageBase.Name = pageTitle;

            if (url != null)
            {
                pageBase.URLSegment = url;
            }

            this.contentRepository.Save(pageBase, SaveAction.Publish, AccessLevel.NoAccess);
            return pageBase.PageLink;


        }
    }
}