using threshold.Connections;
using threshold.Tools;

namespace threshold.Applications
{
    public class WindowsApplication : IApplication
    {
        private IConnection _Connection;
        private string _Md5Hash;
        private string _Name;

        public WindowsApplication(IConnection connection)
        {
            _Connection = connection;
        }

        public bool IsSystemOwned
        {
            get
            {
                if ("".Equals(_Connection.OwnerExecutablePath))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public int Pid
        {
            get
            {
                return _Connection.OwnerPid;
            }
        }

        public IConnection Connection
        {
            get
            {
                return _Connection;
            }
        }

        public string ExecutablePath
        {
            get
            {
                return _Connection.OwnerExecutablePath;
            }
        }

        public string Md5Hash
        {
            get
            {
                if (_Md5Hash == null)
                {
                    _Md5Hash = DataHelper.GetMd5Hash(ExecutablePath);
                }
                return _Md5Hash;
            }
        }

        public string Name
        {
            get
            {
                if (_Name == null)
                {
                    string executablePath = _Connection.OwnerExecutablePath;
                    if ("".Equals(executablePath))
                    {
                        _Name = "Unknown";
                    }
                    else
                    {
                        _Name = executablePath.Substring(executablePath.LastIndexOf('\\') + 1);
                    }
                }
                return _Name;
            }
        }
    }
}
