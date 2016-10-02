using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using threshold.Tools;

namespace threshold.Apis.VirusTotal.Requests
{
    public abstract class BaseRequest : IRequest
    {
        public bool ReceivedResponseFromServer { get; set; } = false;

        public JObject ServerResponse { get; set; } = new JObject { };

        protected readonly string BaseUrl = "http://www.virustotal.com";

        protected abstract string GetUri();

        protected abstract string GetHttpMethod();

        protected abstract NameValueCollection Contents { get; set; }

        public void ExecuteSynchronously()
        {
            JObject jObject = new JObject { };
            string url = BaseUrl + "/" + GetUri();
            WebClient webClient = new WebClient();

            try
            {
                byte[] responseBytes = webClient.UploadValues(url, GetHttpMethod(), Contents);
                string info = Encoding.UTF8.GetString(responseBytes);
                jObject = JObject.Parse(info);
                ReceivedResponseFromServer = true;
            }
            catch (Exception e) when (e is ArgumentNullException || e is WebException)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }

            ServerResponse = jObject;
        }

        public int GetResponseCode()
        {
            string value = GetValueFromMessage("response_code", ServerResponse);
            return Data.ToInt(value);
        }

        protected string GetValueFromMessage(string key, JObject message)
        {
            string value = "";
            JToken jToken = null;

            if (key != null && message != null && message.TryGetValue(key, out jToken))
            {
                if (jToken != null)
                {
                    value = jToken.ToString();
                }
            }

            return value;
        }

        public abstract string GetResult();
    }
}
