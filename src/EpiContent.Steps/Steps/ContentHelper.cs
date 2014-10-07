using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using EpiControlTestingApi.Common;
using Newtonsoft.Json;
using TechTalk.SpecFlow;

namespace EpiContent.Steps.Steps
{
    public class ContentHelper
    {
        private const string PageUrl = "/epicontent/page";

        public static PageDto CreatePage(string title, string template, Table data = null)
        {
            Console.WriteLine("-> Creating a " + template + " page called " + title);

            var dto = new PageDto {Name = title, PageType = template, Data = new Dictionary<string, string>()};

            if (data != null)
            {
                foreach (var row in data.Rows)
                {
                    dto.Data.Add(row[0].Replace(" ", string.Empty), row[1]);
                }
            }

            return Send(dto, PageUrl);
        }

        private static T Send<T>(T dto, string url)
        {
            var json = JsonConvert.SerializeObject(dto);
            var bytes = Encoding.UTF8.GetBytes(json);
            return Send<T>(bytes, url);
        }

        private static T Send<T>(byte[] bytes, string url)
        {
            ServicePointManager.DefaultConnectionLimit = 200;
            var request = WebRequest.Create(ConfigurationManager.AppSettings["SiteUrl"] + url);
            string result;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            request.Timeout = 20000;
            request.ContentLength = bytes.Length;
            var requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();

            try
            {
                using (var response = request.GetResponse())
                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    string value;
                    using (var reader = new StreamReader(ex.Response.GetResponseStream(), Encoding.UTF8))
                    {
                        value = reader.ReadToEnd();
                    }
                    
                    throw new ApplicationException("An error occurred creating the content\n" + request.RequestUri.AbsoluteUri + "\n" + request.Headers + "\n\n" + value, ex);
                }

                throw new ApplicationException("An unknown error occurred creating the content\n" + ex.Message + " Status: " + ex.Status + "\n" + ex.StackTrace);
            }

            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}