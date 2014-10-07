using System;
using EpiControlTestingApi.Common;

namespace EpiContentTestingApi.Services
{
    public interface IBlockService
    {
        BlockDto Add(BlockDto block);
    }

    public class BlockService : IBlockService
    {
        public BlockDto Add(BlockDto block)
        {
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
    }
}