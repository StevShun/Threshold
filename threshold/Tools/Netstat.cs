using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections.Concurrent;

namespace threshold.Tools
{
    public class Netstat
    {
        public List<Line> Output { get; private set; }

        public Netstat()
        {
            List<string> rawOutput = Execute();
            rawOutput.RemoveRange(0, 4);
            var tmpBag = new ConcurrentBag<Line>();

            Parallel.ForEach(rawOutput, (line) =>
            {
                Line netstatLine = new Line(line);
                tmpBag.Add(netstatLine);
            });

            this.Output = new List<Line>();
            this.Output.AddRange(tmpBag);
        }

        public List<string> Execute()
        {
            return CommandLine.ExecuteCommand("netstat", "-ano");
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
            private string[] SubStrings { get; set; }

            public Line(string netstatLine)
            {
                this.SubStrings = netstatLine.Split(new[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries).ToArray();

                this.Pid = GetPid();
                this.ForeignPort = GetForeignPort();
                this.LocalPort = GetLocalPort();
                this.ForeignAddress = GetForeignAddress();
                this.LocalAddress = GetLocalAddress();
                this.Proto = GetProto();
                this.State = GetState();
            }

            private int GetPid()
            {
                if (this.SubStrings.Length > 0)
                {
                    return DataHelper.ToInt(
                        this.SubStrings[this.SubStrings.Length - 1]);
                }
                else
                {
                    return 0;
                }
            }

            private int GetForeignPort()
            {
                if (this.SubStrings.Length >= 3)
                {
                    return DataHelper.ToInt(
                        this.SubStrings[2].Substring(
                        this.SubStrings[2].LastIndexOf(":") + 1));
                }
                else
                {
                    return 0;
                }
            }

            private int GetLocalPort()
            {
                if (this.SubStrings.Length >= 2)
                {
                    return DataHelper.ToInt(
                        this.SubStrings[1].Substring(
                        this.SubStrings[1].LastIndexOf(":") + 1));
                }
                else
                {
                    return 0;
                }
            }

            private string GetForeignAddress()
            {
                if (this.SubStrings.Length >= 3)
                {
                    return this.SubStrings[2].Substring(
                        0, this.SubStrings[2].LastIndexOf(":"));
                }
                else
                {
                    return "Unknown";
                }
            }

            private string GetLocalAddress()
            {
                if (this.SubStrings.Length >= 2)
                {
                    return this.SubStrings[1].Substring(
                        0, this.SubStrings[1].LastIndexOf(":"));
                }
                else
                {
                    return "Unknown";
                }
            }

            private string GetProto()
            {
                if (this.SubStrings.Length > 0)
                {
                    return this.SubStrings[0];
                }
                else
                {
                    return "Unknown";
                }
            }

            private string GetState()
            {
                if (this.SubStrings.Length.Equals(5))
                {
                    return this.SubStrings[3];
                }
                else
                {
                    return "Unknown";
                }
            }
        }
    }
}
