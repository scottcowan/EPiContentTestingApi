using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace EpiContent.Api.IntegrationTests
{
    public static class ApiGateway
    {
        public static string Send<T>(T payload, string url, string action = "POST")
        {
            var payloadJson = JsonConvert.SerializeObject(payload);
            var payloadBytes = Encoding.UTF8.GetBytes(payloadJson);
            return Send(payloadBytes, url, action);
        }

        public static string Send(byte[] bytes, string url, string action = "POST")
        {
            ServicePointManager.DefaultConnectionLimit = 200;
            var request = CreateRequest(bytes, url, action);
            try
            {
                return GetResponse(request.GetResponse());
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                {
                    throw new Exception(
                        string.Format("An unknown error occurred\n{0} Status{1}\n{2}", ex.Message, ex.Status,
                            ex.StackTrace), ex);
                }

                var headers = request.Headers.ToString();
                var response = GetResponse(ex.Response);
                throw new Exception(
                    string.Format("There was a problem with the request\n{0}\n\n{1}\n\n{2}",
                        request.RequestUri.AbsoluteUri, headers, response), ex);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("An unknown error occurred\n{0}\n{1}", ex.Message,
                        ex.StackTrace), ex);
            }
        }

        private static string GetResponse(WebResponse response)
        {
                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
        }

        private static WebRequest CreateRequest(byte[] bytes, string url, string action)
        {
            var request = WebRequest.Create(SiteConfiguration.Url + url);
            request.Method = action;
            request.ContentType = "application/json; charset=utf-8";
            request.Timeout = 20000;
            request.ContentLength = bytes.Length;
            var data = request.GetRequestStream();
            data.Write(bytes, 0, bytes.Length);
            data.Close();
            return request;
        }
    }
}