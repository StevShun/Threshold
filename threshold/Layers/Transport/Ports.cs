using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;

namespace threshold.Layers.Transport
{
    class Ports
    {
        public int[] AllListeningPorts;
        public int[] TcpListeningPorts;
        public int[] UdpListeningPorts;

        public Ports() {

        } 

        private static IPGlobalProperties IpGlobalProperties = 
            IPGlobalProperties.GetIPGlobalProperties();

        public IPEndPoint[] GetActiveTcpListeners()
        {
            return IpGlobalProperties.GetActiveTcpListeners();
        }

        private IPEndPoint[] GetActiveUdpListeners()
        {
            return IpGlobalProperties.GetActiveUdpListeners();
        }

        private List<IPEndPoint> GetAllActiveListeners()
        {
            var allActiveListeners = new List<IPEndPoint>();
            allActiveListeners.AddRange(GetActiveTcpListeners());
            allActiveListeners.AddRange(GetActiveUdpListeners());

            return allActiveListeners;
        }




        private string[] GetAllListeningPorts()
        {
            //foreach (IPEndPoint ipEndpoint in Get)
            return null;
        }


    }
}
