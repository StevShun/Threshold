using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace threshold.Apis.VirusTotal.Requests
{
    public abstract class BaseRequest : IRequest
    {
        public bool ReceivedResponseFromServer { get; set; }
        public Exception RuntimeException
        {
            get
            {
                if (RuntimeException != null)
                {
                    return RuntimeException;
                } else
                {
                    return new Exception("Dummy exception.");
                }
            }

            set
            {
                RuntimeException = value;
            }
        }
        protected readonly string BaseUrl = "http://www.virustotal.com";
        protected abstract string GetUri();
        protected abstract string GetHttpMethod();
        protected abstract NameValueCollection Contents { get; set; }

        public string GetServerResponse()
        {
            ReceivedResponseFromServer = true;
            string url = BaseUrl + "/" + GetUri();
            WebClient webClient = new WebClient();
            string info = "";

            try
            {
                byte[] responseBytes = webClient.UploadValues(url, GetHttpMethod(), Contents);
                info = Encoding.UTF8.GetString(responseBytes);
            }
            catch (Exception e)
            {
                ReceivedResponseFromServer = false;
                RuntimeException = e;
            }

            return info;
        }
    }
}
