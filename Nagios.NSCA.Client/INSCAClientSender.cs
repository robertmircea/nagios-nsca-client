using System;

namespace Nagios.NSCA.Client
{
    public interface INSCAClientSender
    {
        /// <summary>
        /// Send a passive check described by message payload to a Nagios NSCA daemon
        /// </summary>        
        bool SendPassiveCheck(Level level, string hostName, string serviceName, string message);
    }
}