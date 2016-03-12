using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;

namespace threshold.Tools
{
    public static class CommandLine
    {
        public static List<string> ExecuteCommandWithArgs(
            string command,
            string arguments)
        {
            // CLI code example sourced from: http://stackoverflow.com/a/206366
            Process process = new Process();
            process.StartInfo.FileName = command;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;

            string output;
            try
            {
                process.Start();
                output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
            catch
            {
                return Data.ToList("");
            }

            return Data.ToList(output);
        }
    }
}
