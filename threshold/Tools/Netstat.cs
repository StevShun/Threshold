using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace threshold.Tools
{
    public class Netstat
    {
        public List<Line> Output { get; private set; }
        private DataHelper dataHelper = new DataHelper();

        public Netstat()
        {
            List<string> rawOutput = Execute();
            rawOutput.RemoveRange(0, 4);
            this.Output = new List<Line>();

            foreach (string line in rawOutput)
            {
                Line netstatLine = new Line(line);
                Output.Add(netstatLine);
            }
        }

        public List<string> Execute()
        {
            CommandLine commandLine = new CommandLine();

            return commandLine.ExecuteCommand("netstat", "-ano");
        }

        public class Line
        {
            public int Pid { get; private set; }
            public int ForeignPort { get; private set; }
            public int LocalPort { get; private set; }
            public string ForeignAddress { get; private set; }
            public string LocalAddress { get; private set; }
            public string Proto { get; private set; }
            public string State { get; private set; }
            private DataHelper dataHelper = new DataHelper();

            public Line(string netstatLine)
            {
                string[] lineSubStrings = netstatLine.Split(new[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries).ToArray();

                this.Pid = GetPid(lineSubStrings);
                this.ForeignPort = GetForeignPort(lineSubStrings);
                this.LocalPort = GetLocalPort(lineSubStrings);
                this.ForeignAddress = GetForeignAddress(lineSubStrings);
                this.LocalAddress = GetLocalAddress(lineSubStrings);
                this.Proto = GetProto(lineSubStrings);
                this.State = GetState(lineSubStrings);
            }

            private int GetPid(string[] subStrings)
            {
                if (subStrings.Length > 0)
                {
                    return dataHelper.ToInt(subStrings[subStrings.Length - 1]);
                }
                else
                {
                    return 0;
                }
            }

            private int GetForeignPort(string[] subStrings)
            {
                if (subStrings.Length >= 3)
                {
                    return dataHelper.ToInt(subStrings[2].Substring
                        (subStrings[2].LastIndexOf(":") + 1));
                }
                else
                {
                    return 0;
                }
            }

            private int GetLocalPort(string[] subStrings)
            {
                if (subStrings.Length >= 2)
                {
                    return dataHelper.ToInt(subStrings[1].Substring
                        (subStrings[1].LastIndexOf(":") + 1));
                }
                else
                {
                    return 0;
                }
            }

            private string GetForeignAddress(string[] subStrings)
            {
                if (subStrings.Length >= 3)
                {
                    return subStrings[2].Substring(0, subStrings[2].LastIndexOf(":"));
                }
                else
                {
                    return "Unknown";
                }
            }

            private string GetLocalAddress(string[] subStrings)
            {
                if (subStrings.Length >= 2)
                {
                    return subStrings[1].Substring(0, subStrings[1].LastIndexOf(":"));
                }
                else
                {
                    return "Unknown";
                }
            }

            private string GetProto(string[] subStrings)
            {
                if (subStrings.Length > 0)
                {
                    return subStrings[0];
                }
                else
                {
                    return "Unknown";
                }
            }

            private string GetState(string[] subStrings)
            {
                if (subStrings.Length >= 4)
                {
                    return subStrings[3];
                }
                else
                {
                    return "Unknown";
                }
            }
        }
    }
}
