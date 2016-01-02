using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;

namespace threshold.Types.Transport
{
    class Ports
    {
        public Dictionary<int, List<string>> ActivePorts = 
            new Dictionary<int, List<string>>();

        public IPGlobalProperties IpGlobalProperties = 
            IPGlobalProperties.GetIPGlobalProperties();

        private IPEndPoint[] getActiveTcpListeners()
        {
            IPGlobalProperties igp = IpGlobalProperties;
            return igp.GetActiveTcpListeners();
        }

        private IPEndPoint[] getActiveUdpListeners()
        {
            IPGlobalProperties ipg = IpGlobalProperties;
            return ipg.GetActiveUdpListeners();
        }
    }
}
