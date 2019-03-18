using AppBeeWebAPI;
using Microsoft.Owin.Hosting;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ServerWarningWebAPI
{
    class Program
    {
        static void Main(string[] args)
        {   
            //var address = GetPhysicalIPAdress();
            var ip = new IPEndPoint(IPAddress.Loopback, 12345);
            var sip = "http://" + ip.ToString();

            using (WebApp.Start<Startup>(sip))
            {
                Console.WriteLine("Server is online in: " + sip);
                Console.ReadLine();
            }
        }

        public static string GetPhysicalIPAdress()
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces().Where(_ => _.OperationalStatus == OperationalStatus.Up))
            {
                var addr = ni.GetIPProperties().GatewayAddresses.FirstOrDefault();
                if (addr != null && !addr.Address.ToString().Equals("0.0.0.0"))
                {
                    if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                return ip.Address.ToString();
                            }
                        }
                    }
                }
            }
            return String.Empty;
        }
    }
}
