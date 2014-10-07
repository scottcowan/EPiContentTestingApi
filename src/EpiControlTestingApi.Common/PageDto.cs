using System.Collections.Generic;

namespace EpiControlTestingApi.Common
{
    public class PageDto
    {
        public string Name { get; set; }
        public string PageType { get; set; }
        public string Language { get; set; }
        public IDictionary<string, string> Data { get; set; }
    }
}