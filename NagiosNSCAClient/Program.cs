using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Nagios.NSCA.Client;

namespace NagiosNSCAClient
{
    class Program
    {
        static void Main(string[] args)
        {
            NSCASettings settings = new NSCASettings()
                                          {
                                              EncryptionType = NSCAEncryptionType.TripleDES,
                                              //                                              NSCAAddress = IPAddress.Parse("172.16.49.218"),
                                              NSCAAddress = args.Length >0 ? args[0] : "172.16.76.100",
                                              Password = "Eith2Ois"
                                          };
            INSCAClientSender sender = new NSCAClientSender(settings);
            sender.SendPassiveCheck(Level.Critical, "bilbao", "SmartSMS", "System failure");
            sender.SendPassiveCheck(Level.OK, "bilbao", "SmartSMS", string.Empty);
            sender = new NSCAClientSender(); //read config from app.config
            sender.SendPassiveCheck(Level.Warning, "bilbao", "SmartSMS", "");
            sender.SendPassiveCheck(Level.Unknown, "bilbao", "SmartSMS", string.Empty);
            Console.WriteLine("message(s) were sent");
        }
    }
}
