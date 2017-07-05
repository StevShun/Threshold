using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using threshold.Applications;

namespace threshold.Apis.VirusTotal.Requests
{
    public class MultiHashRequest : BaseRequest
    {
        private Dictionary<String, IApplication> HashesToApplications;

        public MultiHashRequest(string apikey)
        {
            HashesToApplications = new Dictionary<string, IApplication>();
            Contents.Add(nameof(apikey), apikey);
        }

        public bool AddApplication(IApplication application)
        {
            bool isAdded = false;

            if (HashesToApplications.Count <= 25
                && !HashesToApplications.ContainsKey(application.Md5Hash))
            {
                HashesToApplications.Add(application.Md5Hash, application);
            }

            return isAdded;
        }

        public override void Build()
        {
            StringBuilder stringBuilder = new StringBuilder();
            int count = 0;
            foreach (string hash in HashesToApplications.Keys)
            {
                stringBuilder.Append(hash);
                if (count != HashesToApplications.Count)
                {
                    stringBuilder.Append(", ");
                }
                count++;
            }
            Contents.Add("resource", stringBuilder.ToString());
        }

        protected override string GetUri()
        {
            return "vtapi/v2/file/report";
        }

        protected override string GetHttpMethod()
        {
            return "POST";
        }

        public override string GetServerResponse()
        {
            return GetResponseJsonArray().ToString();
        }

        public override Dictionary<IApplication, Dictionary<string, string>> GetResults()
        {
            Dictionary<IApplication, Dictionary<string, string>> results = new Dictionary<IApplication, Dictionary<string, string>>();

            foreach (JToken jToken in GetResponseJsonArray())
            {
                string resource = jToken.Value<string>(nameof(resource)) ?? "";
                IApplication application = null;
                foreach (IApplication app in HashesToApplications.Values)
                {
                    if (app.Md5Hash == resource)
                    {
                        application = app;
                    }
                }

                Dictionary<string, string> attributes = new Dictionary<string, string>();
                string permalink = jToken.Value<string>(nameof(permalink)) ?? "";
                attributes[nameof(permalink)] = permalink;
                string total = jToken.Value<string>(nameof(total)) ?? "";
                attributes[nameof(total)] = total;
                string response_code = jToken.Value<string>(nameof(response_code)) ?? "";
                attributes[nameof(response_code)] = response_code;
                string positives = jToken.Value<string>(nameof(positives)) ?? "";
                attributes[nameof(positives)] = positives;

                results[application] = attributes;
            }

            return results;
        }
    }
}
