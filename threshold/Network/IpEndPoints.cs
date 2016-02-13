using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;

namespace threshold.Network
{
    class IpEndPoints
    {
        private static IPGlobalProperties IpGlobalProperties =
            IPGlobalProperties.GetIPGlobalProperties();

        public List<IPEndPoint> GetActiveTcpListeners()
        {
            return IpGlobalProperties.GetActiveTcpListeners().ToList<IPEndPoint>();
        }

        public List<IPEndPoint> GetActiveUdpListeners()
        {
            return IpGlobalProperties.GetActiveUdpListeners().ToList<IPEndPoint>();
        }

        public List<IPEndPoint> GetAllActiveListeners()
        {
            var allActiveListeners = new List<IPEndPoint>();
            allActiveListeners.AddRange(GetActiveTcpListeners());
            allActiveListeners.AddRange(GetActiveUdpListeners());

            return allActiveListeners;
        }
    }
}
