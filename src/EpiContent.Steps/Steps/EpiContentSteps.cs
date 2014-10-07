using TechTalk.SpecFlow;

namespace EpiContent.Steps.Steps
{
    [Binding]
    public class EpiContentSteps
    {
        [Given(@"We create a page called (.*) with a (.*) block and it has")]
        [Given(@"These is a page called (.*) with a block and it has")]
        public void GivenThereIsANamedPageWithANamedBlockAndItHas(string title, string block, Table data)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"There is a page using the (.*) template")]
        public void GivenThereIsAPaeUsingATemplate(string template)
        {
            ContentHelper.CreatePage(template, template.Replace(" ", string.Empty));
        }
    }
}