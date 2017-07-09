using System.Collections.Generic;
using System.Diagnostics;

namespace threshold.Tools
{
    public class CommandLine
    {
        public CommandLine()
        {

        }

        public List<string> ExecuteCommandWithArguments(string command, string arguments)
        {
            // CLI code example sourced from: http://stackoverflow.com/a/206366
            Process process = GetProcess(command, arguments);

            string output;
            try
            {
                process.Start();
                output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
            catch
            {
                output = "";
            }

            return DataHelper.ToList(output);
        }

        private Process GetProcess(string command, string arguments)
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
