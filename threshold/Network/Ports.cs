using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;

namespace threshold.Network
{
    public class Ports
    {
        private Network.IpEndPoints IpEndPoints = new Network.IpEndPoints();

        private List<int> GetPortsFromIpEndPoints(List<IPEndPoint> IpEndPoints)
        {  
            List<int> ports = new List<int>();

            foreach (IPEndPoint iep in IpEndPoints)
            {
                ports.Add(iep.Port);
            }

            return ports;
        }

        public List<int> GetTcpListeningPorts()
        {
            List<int> tcpListeningPorts = 
                GetPortsFromIpEndPoints(IpEndPoints.GetActiveTcpListeners());

            return tcpListeningPorts;
        }

        public List<int> GetUdpListeningPorts()
        {
            List<int> udpListeningPorts =
                GetPortsFromIpEndPoints(IpEndPoints.GetActiveUdpListeners());

            return udpListeningPorts;
        }

        public List<int> GetAllListeningPorts()
        {
            List<int> allListeningPorts = new List<int>();

            allListeningPorts.AddRange(GetTcpListeningPorts());
            allListeningPorts.AddRange(GetUdpListeningPorts());

            return allListeningPorts;
        }
    }
}
