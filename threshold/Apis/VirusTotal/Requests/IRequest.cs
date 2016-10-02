using System;

namespace threshold.Apis.VirusTotal.Requests
{
    interface IRequest
    {
        bool ReceivedResponseFromServer { get; set; }

        Exception RuntimeException { get; set; }

        string GetServerResponse();
    }
}
