using System.Linq;
using System.Management;

namespace threshold.Producers.Connections
{
    public class Connection : IConnection
    {
        public int OwnerPid { get; set; }
        public int ExternalPort { get; set; }
        public int LocalPort { get; set; }
        public string ExternalAddress { get; set; }
        public string LocalAddress { get; set; }
        public string Protocol { get; set; }
        public string State { get; set; }
        public string OwnerExecutablePath
        {
            get
            {
                return getExecutablePath();
            }
        }

        private string getExecutablePath()
        {
            string executablePath = "";
            if (OwnerPid > 0)
            {
                string query = "SELECT ExecutablePath FROM Win32_Process WHERE ProcessId = " + OwnerPid;
                using (ManagementObjectSearcher mos = new ManagementObjectSearcher(query))
                {
                    using (ManagementObjectCollection moc = mos.Get())
                    {
                        executablePath = (from mo in moc.Cast<ManagementObject>() select mo["ExecutablePath"]).First().ToString();
                    }
                }
            }
            return executablePath;
        }
    }
}
