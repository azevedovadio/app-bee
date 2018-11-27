using AppBee.Configuration;
using System;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace AppBee.Persistence
{
    public class AppBeeSocketConsumer : IAppBeeConsumer
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType?.FullName);

        public void SendMessage(string textToSend)
        {
            Logger.Info("Sending : " + textToSend);

            TcpClient client = new TcpClient(AppConfiguration.Instance.HostName, AppConfiguration.Instance.HostPort);

            NetworkStream nwStream = client.GetStream();

            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

            client.Close();
        }
    }
}
