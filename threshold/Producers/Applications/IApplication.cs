using System.Diagnostics;

namespace threshold.Producers.Applications
{
    public interface IApplication
    {
        bool IsSystemOwned { get; }

        int Pid { get; }

        Process RunningProcess { get; }

        string ExecutablePath { get; }

        string Md5Hash { get; }

        string Name { get; }
    }
}
