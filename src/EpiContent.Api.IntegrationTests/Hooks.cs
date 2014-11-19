using System;
using TechTalk.SpecFlow;

namespace EpiContent.Api.IntegrationTests
{
    [Binding]
    public static class Hooks
    {
        [BeforeTestRun]
        public static void PreSetup()
        {
            WebServer.StartIis();
        }

        [AfterTestRun]
        public static void PostClose()
        {
            WebServer.StopIis();
        }
    }
}