using System.Diagnostics;
using threshold.Tools;

namespace threshold.Software
{
    public class Application
    {
        public bool IsSystemOwned { get; private set; }
        public int Pid { get; private set; }
        public Process WindowsProcess { get; private set; }
        public string ExecutablePath { get; private set; }
        public string Hash { get; private set; }
        public string Name { get; private set; }

        public Application(int pid)
        {
            IsSystemOwned = false;
            Pid = pid;
            WindowsProcess = GetWindowsProcess();
            if (WindowsProcess != null && Pid != 0)
            {
                ExecutablePath = GetExecutablePath();
                Hash = GetHash();
                Name = GetName();
            }
            else
            {
                ExecutablePath = "";
                Hash = "";
                Name = "";
            }
        }

        public Process GetWindowsProcess()
        {
            Process process;
            try
            {
                process = Process.GetProcessById(this.Pid);
            }
            catch
            {
                process = null;
            }

            return process;
        }

        public string GetExecutablePath()
        {
            string execPath;
            try
            {
                execPath = WindowsProcess.MainModule.FileName;
            }
            catch
            {
                Trace.WriteLine("GetExecutablePath catch for " + Pid);
                execPath = "";
            }

            return execPath;
        }

        public string GetName()
        {
            string name;
            try
            {
                name = WindowsProcess.ProcessName;
            }
            catch
            {
                Trace.WriteLine("GetName catch for " + Pid);
                name = "";
            }

            return name;
        }

        public string GetHash()
        {
            return Data.GetMd5Hash(ExecutablePath);
        }
    }
}
