using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EpiContentTestingApi.Services;
using EpiControlTestingApi.Common;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using StructureMap;

namespace EpiContent.Api.Services
{
    public class BlockService : IBlockService
    {
        private readonly IContentRepository contentRepository;
        private readonly PageLookup lookup;
        private readonly IContentLoader contentLoader;

        public BlockService(IContentRepository contentRepository, PageLookup lookup, IContentLoader contentLoader)
        {
            this.contentRepository = contentRepository;
            this.lookup = lookup;
            this.contentLoader = contentLoader;
        }

        public BlockDto Add(BlockDto blockData)
        {
            ValidateRequest(blockData);

            var name = this.FindBlockName(blockData);
            var clone = lookup.GetExistingPage(blockData.Page, this.lookup.HomePageReference);
            clone = clone.CreateWritableClone();
            var parent = this.GetParentBlock(blockData.ParentBlockName, clone);
            var contentArea = this.GetContentArea(clone, parent);
            if (!this.ContentExists(contentArea, name))
            {
                var blockRef = this.CreateBlock(blockData);
                contentArea.Items.Add(new ContentAreaItem {ContentLink = blockRef});
            }
                // block title
                // find page
                // clone page
                // get containing block
                // get content area
                // check if the block already exists
                // create new block
                // clear any content areas
            
                // foreach property in BlockDto
                // Set Name
                // Add categories
                // Set System.
                // Set PageReference
                // Set XHtmlString
                // Set Enum
                // Set Url
                // Set Nested object

                // save clone page (pub,noaccess)
                // save parent block

                throw new Exception();
        }

        private ContentReference CreateBlock(BlockDto blockData)
        {
            string name = this.FindBlockName(blockData);
            var myType = Type.GetType(blockData.BlockType);
            if (myType == null)
            {
                throw new TypeMismatchException("Couldn't find the type " + blockData.BlockType);
            }

            var method = typeof (IContentRepository).GetMethod("GetDefault", new[] {typeof (ContentReference)});
            var generic = method.MakeGenericMethod(myType);
            var blockObject = generic.Invoke(this.contentRepository, new[] {ContentReference.GlobalBlockFolder});
            var castingMethod = typeof(CastingHelper).GetMethod("CastToMyType").MakeGenericMethod(myType);
            var block = castingMethod.Invoke(new CastingHelper(), new[] {blockObject});
            var content = block as IContent;
            content.Name = name;

            foreach (var property in block.GetType().GetProperties().Where(x => x.PropertyType == typeof (ContentArea)))
            {
                var setter = property.SetMethod;
                setter.Invoke(block, new object[] {new ContentArea()});
            }

            foreach (var fullKey in blockData.Data.Keys)
            {
                var key = this.GetStringSegment(fullKey, '.', 0);
                var propertyInfo = block.GetType().GetProperty(key);
                if (propertyInfo != null)
                {
                    var setter = propertyInfo.SetMethod;
                    var getter = propertyInfo.GetMethod;
                    var propType = propertyInfo.PropertyType;

                    if (key == "Name")
                    {
                        content.Name = blockData.Data[key];
                    }
                    else if(propType == typeof(CategoryList))
                    {
                        var categories = GetCategories(blockData, key);
                        foreach (var category in categories)
                        {
                            var categoizable = block as ICategorizable;
                            if (categoizable != null)
                            {
                                categoizable.Category.Add(category);
                            }
                        }
                    }
                    else if (propType == typeof (PageReference))
                    {
                        var containerPage = GetExistingPage(blockData.Data[key], ContentReference.StartPage);
                        if (containerPage != null)
                        {
                            setter.Invoke(block, new object[]{ containerPage.ContentLink})
                        }
                    }
                }
            }
        }

        private PageData GetExistingPage(string pageName, PageReference pageReference)
        {
            return contentRepository.GetChildren<PageData>(pageReference).FirstOrDefault(x => x.Name == pageName);
        }

        private IEnumerable<int> GetCategories(BlockDto blockData, string key)
        {
            var categories = blockData.Data[key].Split(',').Select(x => x.Trim());
            foreach (var category in categories)
            {
                var node = Category.Find(category);
                if (node == null)
                {
                    var root = Category.GetRoot();
                    var child = new Category
                    {
                        Available = true,
                        Description = category,
                        Name = category,
                        GUID = Guid.NewGuid(),
                        Selectable = true,
                        Parent = root,
                        SortOrder = 444,
                        Indent = 1
                    };
                    child.Save();
                    node = Category.Find(category);
                }

                yield return node.ID;
            }
        }

        private string GetStringSegment(string value, char segmentDeliminator, int segmentIndex)
        {
            var segments = value.Split(new[] {segmentDeliminator});
            return segments.Count() > segmentIndex ? segments[segmentIndex] : string.Empty;
        }

        private string FindBlockName(BlockDto blockData)
        {
            foreach (var name in new[] { "Name", "Title" })
            {
                if (blockData.Data.ContainsKey(name))
                {
                    return blockData.Data[name];
                }
            }

            return blockData.BlockType.Split(',').FirstOrDefault().Split('.').LastOrDefault().AddSpacesToSentence();
        }

        public void ValidateRequest(BlockDto blockData)
        {
            try
            {
                Type.GetType(blockData.BlockType);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not find the type" + blockData.BlockType, ex);
            }
        }

        private bool ContentExists(ContentArea contentArea, string name)
        {
            var contents = contentArea.Items.Select(this.GetContent);
            var content = contents.FirstOrDefault(x => x.Name == name);
            return content != null;
        }

        private IContent GetContent(ContentAreaItem x)
        {
            return this.contentRepository.Get<BlockData>(x.ContentLink) as IContent;
        }

        private ContentArea GetContentArea(PageData pageData, IContentData parent)
        {
            if (parent != null)
            {
                return GetFirstContentArea(parent);
            }

            return GetFirstContentArea(pageData);
        }

        private IContentData GetParentBlock(string parentBlock, PageData clone)
        {
            var contentArea = GetFirstContentArea(clone);
        }

        private ContentArea GetFirstContentArea(IContentData clone)
        {
            var property = clone.GetType().GetProperties().FirstOrDefault(x => x.PropertyType == typeof(ContentArea));
            if (property != null)
            {
                var getter = property.GetGetMethod();
                var value = getter.Invoke(clone, new object[] { }) as ContentArea;
                if (value == null)
                {
                    var setter = property.GetSetMethod();
                    value = new ContentArea();
                    setter.Invoke(clone, new object[] { value });
                }

                return value;
            }
        }
    }
}