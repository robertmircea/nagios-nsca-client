using System;
using System.Net;
using System.Configuration;

namespace Nagios.NSCA.Client
{
    /// <summary>
    /// Settings necessary for sending passive checks to Nagios NSCA daemon
    /// </summary>
    public sealed class NSCASettings : ConfigurationSection
    {

        [ConfigurationProperty("nscaAddress", DefaultValue = "127.0.0.1", IsRequired = false)]
        public string NSCAAddress
        {
            get { return (string) this["nscaAddress"]; }
            set { this["nscaAddress"] = value; }
        }


        /// <summary>
        /// The port on which NSCA is listening. Defaults to 5667
        /// </summary>
        [ConfigurationProperty("port", DefaultValue = 5667, IsRequired = false)]
        [IntegerValidator(ExcludeRange = false, MaxValue = 65535, MinValue = 1)]
        public int Port
        {
            get { return (int) this["port"]; }
            set
            {
                if (value < 1 || value > 65535)
                    throw new ArgumentOutOfRangeException("port", "Port value must be between 1 and 65535");
                this["port"] = value;
            }
        }

        /// <summary>
        /// The password configured in the ncsa.cfg file used by NSCA
        /// </summary>
        [ConfigurationProperty("password", DefaultValue = "", IsRequired = false)]
        public string Password
        {
            get { return (string) this["password"]; }
            set { this["password"] = value; }
        }

        [ConfigurationProperty("encryptionType", DefaultValue = NSCAEncryptionType.Xor, IsRequired = false)]
        public NSCAEncryptionType EncryptionType
        {
            get { return (NSCAEncryptionType) this["encryptionType"]; }
            set { this["encryptionType"] = value; }
        }
    }
}