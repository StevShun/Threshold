using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace threshold.Software
{
    public class Application
    {
        public int Pid { get; private set; }
        public string ExecutablePath { get; private set; }
        public string Name { get; private set; }

        public Application(int pid)
        {
            Process process;
            try
            {
                process = Process.GetProcessById(pid);
            }
            catch (ArgumentException e)
            {
                return;
            }
            catch (InvalidOperationException e)
            {
                return;
            }

            string execPath;
            try
            {
                execPath = process.MainModule.FileName;
            }
            catch
            {
                execPath = null;
            }

            this.Pid = pid;
            this.ExecutablePath = execPath;
            this.Name = process.ProcessName;
        }
    }
}
