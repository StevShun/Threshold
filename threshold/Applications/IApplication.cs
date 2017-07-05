using System.Diagnostics;
using threshold.Connections;

namespace threshold.Applications
{
    public interface IApplication
    {
        bool IsSystemOwned { get; }

        int Pid { get; }

        IConnection Connection { get; }

        string ExecutablePath { get; }

        string Md5Hash { get; }

        string Name { get; }
    }
}
