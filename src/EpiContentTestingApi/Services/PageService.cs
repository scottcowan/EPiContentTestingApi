using System;
using System.Linq;
using EpiControlTestingApi.Common;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAccess;
using EPiServer.Security;
using EPiServer.ServiceLocation;

namespace EpiContentTestingApi.Services
{
    public class PageService : IPageService
    {
        private readonly IContentRepository contentRepository;
        private IPageFinder finder;

        public PageService()
        {
            contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();

        }

        public PageDto Add(PageDto data)
        {
            Type pageType = null;
           
            try
            {
                pageType = Type.GetType(data.PageType);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Couldn't find the type of " + data.PageType);
            }

            DeleteExistingPage(data.Name);

            var method = typeof(IContentRepository)
                .GetMethod("GetDefault", new[] { typeof(ContentReference) })
                .MakeGenericMethod(pageType);
            var pageObject = method.Invoke(contentRepository, new[] { ContentReference.StartPage });
            var genericMethod = typeof(CastingHelper).GetMethod("CastToType").MakeGenericMethod(pageType);
            var page = genericMethod.Invoke(new CastingHelper(), new[] { pageObject });
            (page as IContent).Name = data.Name;

            foreach (var pair in data.Data)
            {
                var property = page.GetType().GetProperty(pair.Key);
                if (property != null)
                {
                    var propertyType = property.PropertyType;
                    // set collections (key value items)
                    if (propertyType.IsPrimitive || propertyType == typeof(Decimal))
                    {
                        var convert = typeof(Convert).GetMethod("To" + propertyType.Name, new[] { typeof(string) });
                        var value = convert.Invoke(null, new[] { pair.Value });
                        property.SetMethod.Invoke(page, new [] { value });
                    }
                    else if (propertyType == typeof(string))
                    {
                        property.SetMethod.Invoke(page, new[] {pair.Value});
                    }
                    else if (propertyType == typeof(CategoryList))
                    {
                        var root = Category.GetRoot();
                        var categorizable = page as ICategorizable;

                        foreach (var category in pair.Value.Split(',').Select(x => x.Trim()))
                        {
                            var item = Category.Find(category);
                            if (item == null)
                            {
                                var child = new Category
                                {
                                    Available = true,
                                    Description = category,
                                    Name = category,
                                    GUID = Guid.NewGuid(),
                                    Selectable = true,
                                    Parent = root,
                                    Indent = 1
                                };
                                child.Save();
                                item = Category.Find(category);
                            }
                            
                            categorizable.Category.Add(item.ID);
                        }
                    }
                    else if (propertyType == typeof(XhtmlString))
                    {
                        property.SetMethod.Invoke(page, new object[] { new XhtmlString(pair.Value) });
                    }
                    else if (propertyType == typeof (Url))
                    {
                        property.SetMethod.Invoke(page, new[] { new Url(pair.Value)});
                    }
                }
            }
            contentRepository.Save(page as IContent, SaveAction.Publish, AccessLevel.NoAccess);

            return data;
        }

        private void DeleteExistingPage(string name)
        {
            var existing = finder.Find(name);
            if (existing != null)
            {
                contentRepository.Delete(existing, true, AccessLevel.NoAccess);
            }
        }
    }
}