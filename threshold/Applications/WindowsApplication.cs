using System.ComponentModel;
using System.Diagnostics;
using threshold.Tools;

namespace threshold.Applications
{
    public class WindowsApplication : IApplication
    {
        public WindowsApplication(int pid)
        {
            Pid = pid;
        }

        public bool IsSystemOwned
        {
            get
            {
                return false;
            }
        }

        public int Pid { get; private set; }

        public Process RunningProcess
        {
            get
            {
                Process process = null;
                if (Pid > 0)
                {
                    try
                    {
                        process = Process.GetProcessById(Pid);
                    }
                    catch
                    {
                        // TODO: Actually do something.
                    }
                }

                return process;
            }
        }

        public string ExecutablePath
        {
            get
            {
                string execPath = "";
                if (RunningProcess != null)
                {
                    try
                    {
                        execPath = RunningProcess.MainModule.FileName;
                    }
                    catch (Win32Exception e)
                    {
                        Debug.WriteLine("GetExecutablePath catch for PID " + Pid + " " + e);
                    }
                }

                return execPath;
            }
        }

        public string Md5Hash
        {
            get
            {
                return DataHelper.GetMd5Hash(ExecutablePath);
            }
        }

        public string Name
        {
            get
            {
                string name = "";
                if (RunningProcess != null)
                {
                    try
                    {
                        name = RunningProcess.ProcessName;
                    }
                    catch
                    {
                        Debug.WriteLine("GetName catch for " + Pid);
                    }
                }

                return name;
            }
        }
    }
}
