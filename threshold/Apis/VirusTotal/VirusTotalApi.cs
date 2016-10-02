using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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

            return evil;
        }

        public string RequestHashInfo(string md5Hash)
        {
            HashReportRequest hashReportRequest = new HashReportRequest(ApiKey, md5Hash);
            hashReportRequest.ExecuteSynchronously();
            string result = hashReportRequest.GetResult();

            if (!hashReportRequest.ReceivedResponseFromServer)
            {
                System.Diagnostics.Debug.WriteLine("Request failed.");
            } else
            {
                System.Diagnostics.Debug.WriteLine(hashReportRequest.GetResponseCode());
            }

            return result;
        }
    }
}
