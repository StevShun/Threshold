using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using threshold.Apis.VirusTotal.Requests;
using Newtonsoft.Json.Linq;

namespace threshold.Apis.VirusTotal
{
    public class VirusTotalApi
    {
        private readonly string ApiKey = Properties.Settings.Default.VirusTotalApiKey;

        public bool IsFileEvil(string md5Hash)
        {
            bool evil = false;
            string info = RequestHashInfo(md5Hash);
            JObject json = JObject.Parse(info);
            JToken jToken = null;
            if (json.TryGetValue("positives", out jToken))
            {
                if (jToken != null)
                {
                }
            }

            return evil;
        }

        public string RequestHashInfo(string md5Hash)
        {
            HashReportRequest hashReportRequest = new HashReportRequest(ApiKey, md5Hash);
            string result = hashReportRequest.GetServerResponse();

            if (!hashReportRequest.ReceivedResponseFromServer)
            {
                System.Diagnostics.Debug.WriteLine(hashReportRequest.RuntimeException);
            }

            return result;
        }
    }
}
