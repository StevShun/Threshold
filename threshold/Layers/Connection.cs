using System.Collections.Generic;
using threshold.Tools;

namespace threshold.Layers
{
    public class Connection
    {
        public int OwnerPid { get; private set; }
        public int ExternalPort { get; private set; }
        public int LocalPort { get; private set; }
        public string ExternalAddress { get; private set; }
        public string LocalAddress { get; private set; }
        public string Protocol { get; private set; }
        public string State { get; private set; }

        public Connection()
        {
            OwnerPid = 0;
            ExternalPort = 0;
            LocalPort = 0;
            ExternalAddress = "Unknown";
            LocalAddress = "Unknown";
            Protocol = "Unknown";
            State = "Unknown";
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
                    OwnerPid = line.Pid,
                    Protocol = line.Proto,
                    State = line.State,
                };

                connections.Add(connection);
            }

            return connections;
        }
    }
}
