using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;

namespace threshold.Layers.Transport
{
    public class Ports
    {
        public int[] AllListeningPorts;
        public int[] TcpListeningPorts;
        public int[] UdpListeningPorts;

        public Ports() {
            AllListeningPorts = GetAllListeningPorts();
            TcpListeningPorts = GetTcpListeningPorts();
            UdpListeningPorts = GetUdpListeningPorts();
        }

        private Layers.Network.IpEndPoints IpEndPoints = 
            new Layers.Network.IpEndPoints();

        private int[] GetIpEndPointsPorts(IPEndPoint[] IpEndPoints)
        {
            List<int> ports = new List<int>();

            foreach (IPEndPoint iep in IpEndPoints)
            {
                ports.Add(iep.Port);
            }

            return ports.ToArray();
        }

        private int[] GetTcpListeningPorts()
        {
            int[] tcpListeningPorts = 
                GetIpEndPointsPorts(IpEndPoints.TcpListeningEndPoints);

            return tcpListeningPorts;
        }

        private int[] GetUdpListeningPorts()
        {
            int[] udpListeningPorts =
                GetIpEndPointsPorts(IpEndPoints.UdpListeningEndPoints);

            return udpListeningPorts;
        }

        private int[] GetAllListeningPorts()
        {
            List<int> allListeningPorts = new List<int>();

            allListeningPorts.AddRange(GetTcpListeningPorts());
            allListeningPorts.AddRange(GetUdpListeningPorts());

            return allListeningPorts.ToArray();
        }
    }
}
