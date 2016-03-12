using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using threshold.Tools;

namespace threshold.Software
{
    public class Application
    {
        public int Pid { get; private set; }
        public Process Process { get; private set; }
        public string ExecutablePath { get; private set; }
        public string Hash { get; private set; }
        public string Name { get; private set; }

        public Application(int pid)
        {
            this.Pid = pid;
            this.Process = GetProcess();
            this.ExecutablePath = GetExecutablePath();
            this.Hash = GetHash();
            this.Name = GetName();
        }

        public Process GetProcess()
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
                execPath = this.Process.MainModule.FileName;
            }
            catch
            {
                execPath = "";
            }

            return execPath;
        }

        public string GetName()
        {
            string name;
            try
            {
                name = this.Process.ProcessName;
            }
            catch
            {
                name = "";
            }

            return name;
        }

        public string GetHash()
        {
            return Data.GetMd5Hash(this.ExecutablePath);
        }
    }
}
