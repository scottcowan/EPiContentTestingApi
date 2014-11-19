using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace EpiContent.Api.IntegrationTests
{
    public static class WebServer
    {
        private static Process _iisProcess;

        public static void StartIis()
        {
            if (_iisProcess == null)
            {
                var thread = new Thread(StartIisExpress) {IsBackground = true};
                thread.Start();
            }
        }

        private static void StartIisExpress()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            //var path = Path.Combine(currentDirectory)
            var fullName = Assembly.GetCallingAssembly().FullName;
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Normal,
                ErrorDialog = true,
                LoadUserProfile = true,
                CreateNoWindow = false,
                UseShellExecute = false,
                Arguments =
                    string.Format("/path:\"{0}\" /port:{1}", @"C:\Users\Scott Cowan\Documents\Visual Studio 2013\Projects\EPiContentTestingApi\src\ExampleSite", "8193")
            };
            string programfiles = string.IsNullOrEmpty(startInfo.EnvironmentVariables["programfiles"])
                ? startInfo.EnvironmentVariables["programfiles(x86)"]
                : startInfo.EnvironmentVariables["programfiles"];
            startInfo.FileName = programfiles + "\\IIS Express\\iisexpress.exe";
            //startInfo.CreateNoWindow = true;
            try
            {
                _iisProcess = new Process {StartInfo = startInfo};
                _iisProcess.Start();
                _iisProcess.WaitForExit();
            }
            catch
            {
                _iisProcess.CloseMainWindow();
                _iisProcess.Dispose();
            }
        }

        public static void StopIis()
        {
            if (_iisProcess != null)
            {
                _iisProcess.CloseMainWindow();
                _iisProcess.Dispose();
            }
        }
    }
}