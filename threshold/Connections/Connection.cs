using System;
using System.Diagnostics;
using System.Management;

namespace threshold.Connections
{
    public class Connection : IConnection
    {
        private string _ExecutablePath = "";

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
                if ("".Equals(_ExecutablePath))
                {
                    _ExecutablePath = GetExecutablePath();
                }
                return _ExecutablePath;
            }
        }

        private string GetExecutablePath()
        {
            string executablePath = "";
            if (OwnerPid > 0)
            {
                string query = "SELECT ExecutablePath FROM Win32_Process WHERE ProcessId = " + OwnerPid;
                ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(query);
                object temp = null;
                foreach (ManagementObject managementObject in managementObjectSearcher.Get())
                {
                    temp = managementObject["ExecutablePath"] ?? "";
                }
                if ("".Equals(executablePath))
                {
                    try
                    {
                        Process process = Process.GetProcessById(OwnerPid);
                        executablePath = process.MainModule.FileName;
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            return executablePath;
        }
    }
}
