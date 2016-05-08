using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            Output = new List<Line>();
            Output.AddRange(tmpBag);
        }

        public List<string> Execute()
        {
            return CommandLine.ExecuteCommandWithArgs("netstat", "-ano");
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
                SubStrings = netstatLine.Split(new[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries).ToArray();

                Pid = GetPid();
                ForeignPort = GetForeignPort();
                LocalPort = GetLocalPort();
                ForeignAddress = GetForeignAddress();
                LocalAddress = GetLocalAddress();
                Proto = GetProto();
                State = GetState();
            }

            private int GetPid()
            {
                if (SubStrings.Length > 0)
                {
                    return Data.ToInt(SubStrings[SubStrings.Length - 1]);
                }
                else
                {
                    return 0;
                }
            }

            private int GetForeignPort()
            {
                if (SubStrings.Length >= 3)
                {
                    return Data.ToInt(SubStrings[2].Substring(
                        SubStrings[2].LastIndexOf(":") + 1));
                }
                else
                {
                    return 0;
                }
            }

            private int GetLocalPort()
            {
                if (SubStrings.Length >= 2)
                {
                    return Data.ToInt(SubStrings[1].Substring(
                        SubStrings[1].LastIndexOf(":") + 1));
                }
                else
                {
                    return 0;
                }
            }

            private string GetForeignAddress()
            {
                if (SubStrings.Length >= 3)
                {
                    return SubStrings[2].Substring(0,
                        SubStrings[2].LastIndexOf(":"));
                }
                else
                {
                    return "Unknown";
                }
            }

            private string GetLocalAddress()
            {
                if (SubStrings.Length >= 2)
                {
                    return SubStrings[1].Substring(0,
                        SubStrings[1].LastIndexOf(":"));
                }
                else
                {
                    return "Unknown";
                }
            }

            private string GetProto()
            {
                if (SubStrings.Length > 0)
                {
                    return SubStrings[0];
                }
                else
                {
                    return "Unknown";
                }
            }

            private string GetState()
            {
                if (SubStrings.Length.Equals(5))
                {
                    return SubStrings[3];
                }
                else
                {
                    return "Unknown";
                }
            }
        }
    }
}
