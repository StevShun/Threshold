using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using threshold.Producers.Connections;

namespace threshold.Tools
{
    public class Netstat
    {
        public Netstat()
        {
        }

        public ConcurrentBag<IConnection> GetConnections()
        {
            ConcurrentBag<IConnection> connections = new ConcurrentBag<IConnection>();
            CommandLine commandLine = new CommandLine();
            List<string> rawOutput = commandLine.ExecuteCommandWithArguments("netstat", "-ano");
            // Remove the header that Netstat adds to its output.
            rawOutput.RemoveRange(0, 4);

            Parallel.ForEach(rawOutput, (line) =>
            {
                Line netstatLine = new Line(line);
                Connection connection = new Connection
                {
                    ExternalAddress = netstatLine.ForeignAddress,
                    ExternalPort = netstatLine.ForeignPort,
                    LocalAddress = netstatLine.LocalAddress,
                    LocalPort = netstatLine.LocalPort,
                    OwnerPid = netstatLine.Pid,
                    Protocol = netstatLine.Proto,
                    State = netstatLine.State
                };
                connections.Add(connection);
            });

            return connections;
        }

        public ConcurrentBag<IConnection> GetUniqueConnections()
        {
            ConcurrentBag<IConnection> connections = GetConnections();

            return new ConcurrentBag<IConnection>(connections.GroupBy(x => x.OwnerPid).Select(x => x.First()));
        }

        class Line
        {
            private string[] LineSubStrings;

            public Line(string netstatLine)
            {
                LineSubStrings = netstatLine.Split(new[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries).ToArray();
            }

            public int Pid
            {
                get
                {
                    if (LineSubStrings != null && LineSubStrings.Length > 0)
                    {
                        return DataHelper.ToInt(LineSubStrings[LineSubStrings.Length - 1]);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }

            public int ForeignPort
            {
                get
                {
                    if (LineSubStrings != null && LineSubStrings.Length >= 3)
                    {
                        return DataHelper.ToInt(LineSubStrings[2].Substring(
                            LineSubStrings[2].LastIndexOf(":") + 1));
                    }
                    else
                    {
                        return 0;
                    }
                }
            }

            public int LocalPort
            {
                get
                {
                    if (LineSubStrings != null && LineSubStrings.Length >= 2)
                    {
                        return DataHelper.ToInt(LineSubStrings[1].Substring(
                            LineSubStrings[1].LastIndexOf(":") + 1));
                    }
                    else
                    {
                        return 0;
                    }
                }
            }

            public string ForeignAddress
            {
                get
                {
                    if (LineSubStrings != null && LineSubStrings.Length >= 3)
                    {
                        return LineSubStrings[2].Substring(0,
                            LineSubStrings[2].LastIndexOf(":"));
                    }
                    else
                    {
                        return "Unknown";
                    }
                }
            }

            public string LocalAddress
            {
                get
                {
                    if (LineSubStrings != null && LineSubStrings.Length >= 2)
                    {
                        return LineSubStrings[1].Substring(0,
                            LineSubStrings[1].LastIndexOf(":"));
                    }
                    else
                    {
                        return "Unknown";
                    }
                }
            }

            public string Proto
            {
                get
                {
                    if (LineSubStrings != null && LineSubStrings.Length > 0)
                    {
                        return LineSubStrings[0];
                    }
                    else
                    {
                        return "Unknown";
                    }
                }
            }

            public string State
            {
                get
                {
                    if (LineSubStrings != null && LineSubStrings.Length.Equals(5))
                    {
                        return LineSubStrings[3];
                    }
                    else
                    {
                        return "Unknown";
                    }
                }
            }
        }
    }
}
