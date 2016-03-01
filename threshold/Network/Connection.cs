using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using threshold.Software;
using threshold.Tools;

namespace threshold.Network
{
    public class Connection
    {
        public Application Owner { get; private set; }
        public int ExternalPort { get; private set; }
        public int LocalPort { get; private set; }
        public string ExternalAddress { get; private set; }
        public string LocalAddress { get; private set; }
        public string Protocol { get; private set; }
        public string State { get; private set; }

        public Connection()
        {
            this.Owner = null;
            this.ExternalPort = 0;
            this.LocalPort = 0;
            this.ExternalAddress = "Unknown";
            this.LocalAddress = "Unknown";
            this.Protocol = "Unknown";
            this.State = "Unknown";
        }

        public List<Connection> GetActiveConnections()
        {
            return GetActiveConnectionsUsingNetstat();
        }

        private List<Connection> GetActiveConnectionsUsingNetstat()
        {
            List<Connection> connections = new List<Connection>();
            CommandLine commandLine = new CommandLine();

            foreach (string line in commandLine.ExecuteNetstat("-ano"))
            {
                List<string> subStrs = line.Split(new[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries).ToList();

                // Substring sourced from: http://stackoverflow.com/a/25965143
                string protocol = subStrs[0];

                string localAddr = 
                    subStrs[1].Substring(0, subStrs[1].LastIndexOf(":"));
                string extAddr = 
                    subStrs[2].Substring(0, subStrs[2].LastIndexOf(":"));

                int localPort;
                int extPort;
                try 
                {
                    localPort = Int32.Parse(subStrs[1].Substring
                        (subStrs[1].LastIndexOf(":") + 1));
                    extPort = Int32.Parse(subStrs[2].Substring
                        (subStrs[2].LastIndexOf(":") + 1));
                }
                catch (FormatException e)
                {
                    localPort = 0;
                    extPort = 0;
                }

                string state;
                // Hack for Netstat not giving us a state for UDP.
                if (protocol == "UDP")
                {
                    state = "Unknown";
                }
                else
                {
                    state = subStrs[3];
                }

                int pid;
                try
                {
                    pid = Int32.Parse(subStrs[subStrs.Count - 1]);
                }
                catch (FormatException e)
                {
                    pid = 0;
                }
                
                Application application = new Application(pid);

                Connection connection = new Connection
                {
                    ExternalAddress = extAddr,
                    ExternalPort = extPort,
                    LocalAddress = localAddr,
                    LocalPort = localPort,
                    Owner = application,
                    Protocol = protocol,
                    State = state,
                };

                connections.Add(connection);
            }

            return connections;
        }
    }
}
