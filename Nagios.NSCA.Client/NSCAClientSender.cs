using System;
using System.Net.Sockets;

namespace Nagios.NSCA.Client
{
    public class NSCAClientSender : INSCAClientSender
    {
        private readonly NSCASettings settings;
        private readonly PassiveCheckProtocolWriter protocolWriter;

        public NSCAClientSender()
            : this((NSCASettings) System.Configuration.ConfigurationManager.GetSection("nscaSettings"))
        {
        }

        public NSCAClientSender(NSCASettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            this.settings = settings;

            protocolWriter = new PassiveCheckProtocolWriter(settings);
            
        }

        public bool SendPassiveCheck(Level level, string hostName, string serviceName, string message)
        {
            if (string.IsNullOrEmpty(hostName)) throw new ArgumentNullException("hostName");
            if (serviceName == null) throw new ArgumentNullException("serviceName"); //update as per ChrisGeorg issue opened January 05, 2012, https://github.com/robertmircea/nagios-nsca-client/issues/1
            if (message == null) throw new ArgumentNullException("message");

            try
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    tcpClient.Connect(settings.NSCAAddress, settings.Port);
                    using (var stream = tcpClient.GetStream())
                    {
                        byte[] initVector = new byte[128];
                        stream.Read(initVector, 0, 128);

                        byte[] timestamp = new byte[4];
                        stream.Read(timestamp, 0, 4);

                        var bytesToSend = protocolWriter.EncodeToProtocol(level, timestamp, hostName, serviceName, message, initVector);
                        stream.Write(bytesToSend, 0, bytesToSend.Length);
                    }
                }
                return true;
            }
            catch
            {
                //intentionally swallow exceptions, maybe some logging is required.
                return false;
            }
        }
    }
}