using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using threshold.Apis.VirusTotal.Requests;

namespace threshold.Apis.VirusTotal
{
    public class VirusTotalApi
    {
        private readonly string BaseUrl = "http://www.virustotal.com";
        private readonly string RescanUri = "vtapi/v2/file/report";
        private readonly string ApiKey = Properties.Settings.Default.VirusTotalApiKey;

        public bool IsFileEvil(string md5Hash)
        {
            bool evil = false;

            return evil;
        }

        public string RequestHashInfo(string md5Hash)
        {
            string url = BaseUrl + "/" + RescanUri;
            VirusTotalScanRequest virusTotalScanRequest = new VirusTotalScanRequest(ApiKey, md5Hash);
            WebClient webClient = new WebClient();
            string info = "";

            try
            {
                byte[] responseBytes = webClient.UploadValues(url, "POST", virusTotalScanRequest.Contents);
                info = Encoding.UTF8.GetString(responseBytes);
            } catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }

            return info;
        } 
    }
}
