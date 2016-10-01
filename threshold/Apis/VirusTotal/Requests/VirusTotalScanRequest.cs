using System.Collections.Specialized;

namespace threshold.Apis.VirusTotal.Requests
{
    public class VirusTotalScanRequest
    {
        public NameValueCollection Contents { get; private set; }

        public VirusTotalScanRequest(string apikey, string resource)
        {
            Contents = new NameValueCollection();
            Contents.Add(nameof(apikey), apikey);
            Contents.Add(nameof(resource), resource);
        }
    }
}
