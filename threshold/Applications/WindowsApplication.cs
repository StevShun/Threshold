using threshold.Connections;
using threshold.Tools;

namespace threshold.Applications
{
    public class WindowsApplication : IApplication
    {
        private const string UnknownApplication = "Unknown";
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
                    _Md5Hash = DataHelper.GetFileMd5Hash(ExecutablePath);
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
                        _Name = UnknownApplication;
                    }
                    else
                    {
                        _Name = executablePath.Substring(executablePath.LastIndexOf('\\') + 1);
                    }
                }
                return _Name;
            }
        }

        public override bool Equals(object obj)
        {
            var item = obj as WindowsApplication;

            if (item == null)
            {
                return false;
            }

            if (Name.Equals(UnknownApplication) && item.Name.Equals(UnknownApplication))
            {
                if (Pid.Equals(item.Pid))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return Md5Hash.Equals(item.Md5Hash);
        }

        public override int GetHashCode()
        {
            return Pid;
        }
    }
}
