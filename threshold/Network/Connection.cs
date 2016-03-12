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
            Netstat netstat = new Netstat();

            foreach (Netstat.Line line in netstat.Output)
            {
                Connection connection = new Connection
                {
                    ExternalAddress = line.ForeignAddress,
                    ExternalPort = line.ForeignPort,
                    LocalAddress = line.LocalAddress,
                    LocalPort = line.LocalPort,
                    Owner = new Application(line.Pid),
                    Protocol = line.Proto,
                    State = line.State,
                };

                connections.Add(connection);
            }

            return connections;
        }
    }
}
