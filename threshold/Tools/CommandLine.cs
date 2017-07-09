using System.Collections.Generic;
using System.Diagnostics;

namespace threshold.Tools
{
    public static class CommandLine
    {
        public static List<string> ExecuteCommandLineProcess(Process process)
        {
            // CLI code example sourced from: http://stackoverflow.com/a/206366
            string output;
            try
            {
                process.Start();
                output = process.StandardOutput.ReadToEnd();
                process.WaitForExit(5000);
            }
            catch
            {
                output = "";
            }
            finally
            {
                process.Dispose();
            }

            return DataHelper.ToList(output);
        }

        public static Process GetProcess(string command, string arguments)
        {
            Process process = new Process();
            process.StartInfo.FileName = command;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            return process;
        }
    }
}
