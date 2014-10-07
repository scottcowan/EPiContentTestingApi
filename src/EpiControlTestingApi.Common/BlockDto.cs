using System.Collections.Generic;

namespace EpiControlTestingApi.Common
{
    public class BlockDto
    {
        public string Page { get; set; }
        public string Name { get; set; }
        public string ContentArea { get; set; }
        public string BlockType { get; set; }
        public string ParentBlockName { get; set; }
        public string ParentBlockContentArea { get; set; }
        public IDictionary<string, string> Data { get; set; }
    }
}