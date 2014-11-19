using System.Collections.Generic;
using System.Linq;
using EpiControlTestingApi.Common;
using TechTalk.SpecFlow;

namespace EpiContent.Api.IntegrationTests
{
    [Binding]
    public class StepDefinition1
    {
        [Given(@"I create a (.*) page")]
        public void GivenThereIsAPageUsingTheTemplate(string page)
        {
            ApiGateway.Send(new PageDto
            {
                PageType = "ExampleSite.Models.Pages.{0}Page, ExampleSite"
            },
            "/api/page");
        }
    }

    public static class SiteConfiguration
    {
        public static string Url
        {
            get
            {
                var section = System.Configuration.ConfigurationManager.GetSection("specBind");
                return ((SpecBind.Configuration.ConfigurationSectionHandler)section).Application.StartUrl;
            }
        }
    }
}
