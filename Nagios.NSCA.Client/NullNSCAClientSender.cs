namespace Nagios.NSCA.Client
{
    public class NullNSCAClientSender : INSCAClientSender
    {
        private static readonly NullNSCAClientSender instance = new NullNSCAClientSender();

        public static NullNSCAClientSender Instance { get { return instance; } }

        public bool SendPassiveCheck(Level level, string hostName, string serviceName, string message)
        {
            return true;
        }
    }
}