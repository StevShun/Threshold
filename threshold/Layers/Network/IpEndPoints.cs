using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;

namespace threshold.Layers.Network
{
    class IpEndPoints
    {

        // From Java, I would say that these should be private and not
        // directly accessed which is being done in MainForm.cs
        public IPEndPoint[] AllListeningEndPoints;
        public IPEndPoint[] TcpListeningEndPoints;
        public IPEndPoint[] UdpListeningEndPoints;

        public IpEndPoints()
        {
            AllListeningEndPoints = GetAllActiveListeners();
            TcpListeningEndPoints = GetActiveTcpListeners();
            UdpListeningEndPoints = GetActiveUdpListeners();
        }

        private static IPGlobalProperties IpGlobalProperties = 
            IPGlobalProperties.GetIPGlobalProperties();

        private IPEndPoint[] GetActiveTcpListeners()
        {
            return IpGlobalProperties.GetActiveTcpListeners();
        }

        private IPEndPoint[] GetActiveUdpListeners()
        {
            return IpGlobalProperties.GetActiveUdpListeners();
        }

        private IPEndPoint[] GetAllActiveListeners()
        {
            var allActiveListeners = new List<IPEndPoint>();
            allActiveListeners.AddRange(GetActiveTcpListeners());
            allActiveListeners.AddRange(GetActiveUdpListeners());

            return allActiveListeners.ToArray();
        }
    }
}
