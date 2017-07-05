using System.Collections.Generic;
using threshold.Applications;

namespace threshold.Apis.VirusTotal.Requests
{
    public interface IRequest
    {
        bool ReceivedResponseFromServer { get; set; }

        Dictionary<IApplication, Dictionary<string, string>> GetResults();

        string GetServerResponse();

        void ExecuteSynchronously();

        void Build();
    }
}
