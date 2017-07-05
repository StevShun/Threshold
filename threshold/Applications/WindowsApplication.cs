using threshold.Connections;
using threshold.Tools;

namespace threshold.Applications
{
    public class WindowsApplication : IApplication
    {
        private bool _IsSystemOwned;
        private IConnection _Connection;
        private string _ExecutablePath;
        private string _Md5Hash;
        private string _Name;

        public WindowsApplication(IConnection connection)
        {
            _Connection = connection;
            Pid = connection.OwnerPid;
            _ExecutablePath = connection.OwnerExecutablePath;
        }

        public bool IsSystemOwned
        {
            get
            {
                return _IsSystemOwned;
            }
        }

        public int Pid { get; private set; }

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
                return _ExecutablePath;
            }
        }

        public string Md5Hash
        {
            get
            {
                string md5hash = "";
                if (_Md5Hash != null)
                {
                    md5hash = _Md5Hash;
                }
                else
                {
                    md5hash = DataHelper.GetMd5Hash(ExecutablePath);
                    _Md5Hash = md5hash;
                }
                return md5hash;
            }
        }

        public string Name
        {
            get
            {
                return _ExecutablePath;
            }
        }
    }
}
