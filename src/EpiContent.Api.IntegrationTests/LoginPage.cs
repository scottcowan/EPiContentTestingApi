using OpenQA.Selenium;
using SpecBind.Pages;

namespace EpiContent.Api.IntegrationTests
{
    [PageNavigation("/Util/login.aspx")]
    public class LoginPage
    {
        [ElementLocator(CssSelector = "#LoginControl_UserName")]
        public IWebElement UserName { get; set; }
    }
}