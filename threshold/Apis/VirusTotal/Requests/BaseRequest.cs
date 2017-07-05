using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using threshold.Applications;

namespace threshold.Apis.VirusTotal.Requests
{
    public abstract class BaseRequest : IRequest
    {
        protected readonly string BaseUrl = "http://www.virustotal.com";

        public bool ReceivedResponseFromServer { get; set; } = false;

        protected string RawServerResponse { get; set; } = "";

        protected NameValueCollection Contents { get; set; } = new NameValueCollection();

        protected abstract string GetUri();

        protected abstract string GetHttpMethod();

        public void ExecuteSynchronously()
        {
            string url = BaseUrl + "/" + GetUri();
            WebClient webClient = new WebClient();

            try
            {
                byte[] responseBytes = webClient.UploadValues(url, GetHttpMethod(), Contents);
                ReceivedResponseFromServer = true;
                RawServerResponse = Encoding.UTF8.GetString(responseBytes);
            }
            catch (Exception e) when (e is ArgumentNullException || e is WebException)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        public abstract string GetServerResponse();

        public abstract Dictionary<IApplication, Dictionary<string, string>> GetResults();

        protected JArray GetResponseJsonArray()
        {
            JArray jsonArry = new JArray();

            if (RawServerResponse != null)
            {
                try
                {
                    jsonArry = JArray.Parse(RawServerResponse);
                }
                catch (JsonReaderException e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }

            return jsonArry;
        }

        protected JObject GetResponseJsonObject()
        {
            JObject jsonObject = new JObject();

            if (RawServerResponse != null)
            {
                try
                {
                    jsonObject = JObject.Parse(RawServerResponse);
                }
                catch (JsonReaderException e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }

            return jsonObject;
        }

        public abstract void Build();
    }
}
