using System.Collections.Specialized;

namespace threshold.Apis.VirusTotal.Requests
{
    public class HashReportRequest : BaseRequest
    {
        protected override NameValueCollection Contents { get; set; }

        public HashReportRequest(string apikey, string resource)
        {
            Contents = new NameValueCollection();
            Contents.Add(nameof(apikey), apikey);
            Contents.Add(nameof(resource), resource);
        }

        protected override string GetUri()
        {
            return "vtapi/v2/file/report";
        }

        protected override string GetHttpMethod()
        {
            return "POST";
        }

        public override string GetResult()
        {
            return GetValueFromMessage("positives", ServerResponse);
        }
    }
}
