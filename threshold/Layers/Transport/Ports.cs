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
        // From Java, I would say that these should be private and not
        // directly accessed which is being done in MainForm.cs
        // Actually, do you even need them? Why, can't getTcpListenPorts be public and called? 
        // does the state need to be saved here? 
        // if it does I'd make these private and have public getters for them. When you instantiate this object 
        // call a ports.init() that will populate all the fields for you. 
        public int[] AllListeningPorts;
        public int[] TcpListeningPorts;
        public int[] UdpListeningPorts;

        // Generally I would say that having initialization stuff in a constructor is not 
        // very good. Just instantiate the class and have getTcpListeningPorts() and what not public or have a init().
        public Ports() {
            AllListeningPorts = GetAllListeningPorts();
            TcpListeningPorts = GetTcpListeningPorts();
            UdpListeningPorts = GetUdpListeningPorts();
        }

        // This should be at the top of this class. This is a global variable for the class. Should be with AllListeningPorts above. 
        private Layers.Network.IpEndPoints IpEndPoints = 
            new Layers.Network.IpEndPoints();

        // All methods in java would begin with a lower case letter. ex. getIpEndpoints instead of GetIpEndpoints
        private int[] GetIpEndPointsPorts(IPEndPoint[] IpEndPoints)
        {  
            List<int> ports = new List<int>();

            foreach (IPEndPoint iep in IpEndPoints)
            {
                ports.Add(iep.Port);
            }
           
            // I would suggest returning the List<int> that you already made. (I still think it's better to use the IpEndpoint) 
            // Here you're creating another list based on an array that is already in the List (Look at all the
            // convience methods List provides, it's more useful than a basic array). 
            return ports.ToArray();
        }

        // should be public and return a List
        private int[] GetTcpListeningPorts()
        {
            int[] tcpListeningPorts = 
                GetIpEndPointsPorts(IpEndPoints.TcpListeningEndPoints);

            return tcpListeningPorts;
        }

        // should be public and return a List
        private int[] GetUdpListeningPorts()
        {
            int[] udpListeningPorts =
                GetIpEndPointsPorts(IpEndPoints.UdpListeningEndPoints);

            return udpListeningPorts;
        }

        // should be public and return a List
        private int[] GetAllListeningPorts()
        {
            List<int> allListeningPorts = new List<int>();

            allListeningPorts.AddRange(GetTcpListeningPorts());
            allListeningPorts.AddRange(GetUdpListeningPorts());

            return allListeningPorts.ToArray();
        }
    }
}
