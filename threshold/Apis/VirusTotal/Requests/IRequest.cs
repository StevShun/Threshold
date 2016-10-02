using System;
using Newtonsoft.Json.Linq;

namespace threshold.Apis.VirusTotal.Requests
{
    interface IRequest
    {
        bool ReceivedResponseFromServer { get; set; }

        JObject ServerResponse { get; set; }

        void ExecuteSynchronously();

        int GetResponseCode();

        string GetResult();
    }
}
