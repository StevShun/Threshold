using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using threshold.Tools;

namespace threshold.Network
{
    public class Connection
    {
        public string ExternalAddress { get; set; }
        public string ExternalPort { get; set; }
        public string LocalAddress { get; set; }
        public string LocalPort { get; set; }
        public string Process { get; set; }
        public string Protocol { get; set; }
        public string State { get; set; }

        private CommandLine commandLine = new CommandLine();

        public List<Connection> GetActiveConnections()
        {
            return GetActiveConnectionsUsingNetstat();
        }

        private List<Connection> GetActiveConnectionsUsingNetstat()
        {
            List<Connection> connections = new List<Connection>();

            foreach (string s in commandLine.ExecuteNetstat("-ano"))
            {
                Connection connection = new Connection();

                List<string> subStrs = s.Split(new[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries).ToList();

                // Substring sourced from: http://stackoverflow.com/a/25965143
                connection.Protocol = subStrs[0];

                connection.LocalAddress = 
                    subStrs[1].Substring(0, subStrs[1].LastIndexOf(":"));
                connection.LocalPort = 
                    subStrs[1].Substring(subStrs[1].LastIndexOf(":")+1);

                connection.ExternalAddress = 
                    subStrs[2].Substring(0, subStrs[2].LastIndexOf(":"));
                connection.ExternalPort =
                    subStrs[2].Substring(subStrs[2].LastIndexOf(":")+1);

                // Hack for Netstat not giving us a state for UDP.
                if (connection.Protocol == "UDP")
                {
                    connection.State = "Unknown";
                }
                else
                {
                    connection.State = subStrs[3];
                }

                connection.Process = subStrs[subStrs.Count - 1];

                connections.Add(connection);
            }

            return connections;
        }
    }
}
