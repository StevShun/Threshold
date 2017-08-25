using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using threshold.Connections;

namespace threshold.Tools
{
    public static class Netstat
    {
        public static class Snapshot
        {
            public static ConcurrentBag<IConnection> GetCurrentUniqueConnections()
            {
                ConcurrentBag<IConnection> connections = GetCurrentConnections();

                return new ConcurrentBag<IConnection>(connections.GroupBy(x => x.OwnerPid).Select(x => x.First()));
            }

            public static ConcurrentBag<IConnection> GetCurrentConnections()
            {
                ConcurrentBag<IConnection> connections = new ConcurrentBag<IConnection>();
                Process netstat = CommandLine.GetProcess("netstat", "-ano");
                List<string> rawOutput = CommandLine.ExecuteCommandLineProcess(netstat);

                Parallel.ForEach(rawOutput, (line) =>
                {
                    Line netstatLine = new Line(line);
                    if (netstatLine.IsValid)
                    {
                        IConnection connection = new Connection
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
                    }
                });

                return connections;
            }
        }

        public class Daemon
        {
            private static readonly ILog Log = LogManager.GetLogger(typeof(Daemon));
            private BackgroundWorker _BackgroundWorker;
            private Process NetstatProcess;
            private IDictionary<IConnection, bool> ConnectionsToExportStatus;

            public Daemon()
            {
                _BackgroundWorker = new BackgroundWorker
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };
                // Use netstat with "-ano 1" (all connections and ports in,
                // numerical format with no DNS lookup, include owner PID, and,
                // rerun every second.
                NetstatProcess = CommandLine.GetProcess("netstat", "-ano 1");
                ConnectionsToExportStatus = new ConcurrentDictionary<IConnection, bool>();
            }

            public bool Start()
            {
                bool isStarted = false;
                Log.Info("Starting netstat process...");
                try
                {
                    isStarted = NetstatProcess.Start();
                    Log.Info("Started a netstat process");
                    _BackgroundWorker.DoWork += ProcessStandardOutput;
                    _BackgroundWorker.RunWorkerAsync();
                }
                catch
                {
                    Log.Error("Failed to start a netstat process");
                }

                return isStarted;
            }

            private void ProcessStandardOutput(object sender, DoWorkEventArgs e)
            {
                StreamReader streamReader;
                try
                {
                    streamReader = NetstatProcess.StandardOutput;
                }
                catch
                {
                    Log.Error("Failed to acquire output stream for netstat process");
                    e.Cancel = true;
                    return;
                }

                using (streamReader)
                {
                    string line;
                    while (!_BackgroundWorker.CancellationPending
                        && !NetstatProcess.HasExited
                        && (line = streamReader.ReadLine()) != null)
                    {
                        Line netstatLine = new Line(line);
                        if (netstatLine.IsValid)
                        {
                            IConnection connection = new Connection
                            {
                                ExternalAddress = netstatLine.ForeignAddress,
                                ExternalPort = netstatLine.ForeignPort,
                                LocalAddress = netstatLine.LocalAddress,
                                LocalPort = netstatLine.LocalPort,
                                OwnerPid = netstatLine.Pid,
                                Protocol = netstatLine.Proto,
                                State = netstatLine.State
                            };
                            if (!ConnectionsToExportStatus.ContainsKey(connection))
                            {
                                Log.Info("Adding connection...");
                                ConnectionsToExportStatus.Add(connection, false);
                            }
                        }
                    }
                }

                if (NetstatProcess.HasExited && !_BackgroundWorker.CancellationPending)
                {
                    Log.Warn("Netstat process has exited unexpectedly while " +
                        "processing standard output");
                }
                else
                {
                    Log.Info("Netstat is stopping. Killing process...");
                    NetstatProcess.Kill();
                }
                e.Cancel = true;
            }

            public bool Stop()
            {
                bool isStopped = false;
                Log.Info("Stopping netstat process...");
                try
                {
                    _BackgroundWorker.CancelAsync();
                    isStopped = NetstatProcess.WaitForExit(5000);
                    Log.Info("Stopped the netstat process");
                }
                catch
                {
                    Log.Error("Failed to wait for Netstat daemon to exit");
                }
                finally
                {
                    NetstatProcess.Dispose();
                }

                return isStopped;
            }

            public bool HasExited
            {
                get
                {
                    return NetstatProcess.HasExited;
                }
            }

            public IConnection TryGetConnection()
            {
                foreach (KeyValuePair<IConnection, bool> keyValuePair in ConnectionsToExportStatus)
                {
                    if (!keyValuePair.Value) {
                        ConnectionsToExportStatus[keyValuePair.Key] = true;
                        return keyValuePair.Key;
                    }
                }
                return null;
            }
        }

        protected class Line
        {
            private string[] LineSubStrings;
            private bool _IsValid;

            public Line(string netstatLine)
            {
                _IsValid = true;
                LineSubStrings = netstatLine.Split(new[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries).ToArray();
                SetValidity();
            }

            private void SetValidity()
            {
                if (LineSubStrings == null || LineSubStrings.Length < 5)
                {
                    _IsValid = false;
                }
                else if ("".Equals(LineSubStrings[0]))
                {
                    _IsValid = false;
                }
                else if ("Active".Equals(LineSubStrings[0]))
                {
                    _IsValid = false;
                }
                else if ("Proto".Equals(LineSubStrings[0]))
                {
                    _IsValid = false;
                }
            }

            public bool IsValid
            {
                get
                {
                    return _IsValid;
                }
            }

            public int Pid
            {
                get
                {
                    if (_IsValid)
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
                    if (_IsValid)
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
                    if (_IsValid)
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
                    if (_IsValid)
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
                    if (_IsValid)
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
                    if (_IsValid)
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
                    if (_IsValid)
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
