using System;
using System.IO;
using System.Text;

namespace Nagios.NSCA.Client
{
    public class PassiveCheckProtocolWriter
    {
        private readonly NSCASettings settings;
        private const short NSCA_VERSION = 3;
        private const int PLUGIN_OUTPUT_SIZE = 512;
        private const int HOST_NAME_SIZE = 64;
        private const int SERVICE_NAME_SIZE = 128;
        

        public PassiveCheckProtocolWriter(NSCASettings settings)
        {
            this.settings = settings;
        }

        public byte[] EncodeToProtocol(Level level, byte[] timestamp, string hostName, string serviceName, string message, byte[] initVector)
        {
            using (var stream = new MemoryStream(16 + HOST_NAME_SIZE + SERVICE_NAME_SIZE + PLUGIN_OUTPUT_SIZE))
            {
                stream.WriteShort(NSCA_VERSION); //bytes 0-1
                stream.WriteShort(0); //bytes 2-3
                stream.WriteInt(0); //bytes 4-8
                stream.Write(timestamp, 0, 4); //bytes 9-13
                stream.WriteShort((short) level); //bytes 14-15                
                stream.WriteFixedString(hostName, HOST_NAME_SIZE);
                stream.WriteFixedString(serviceName, SERVICE_NAME_SIZE);
                stream.WriteFixedString(message, PLUGIN_OUTPUT_SIZE);
                stream.WriteShort(0);


                int hash = CRC32.Compute(stream.ToArray());
                stream.Position = 4;
                stream.WriteInt(hash);

                var encryptor = EncryptorFactory.CreateEncryptor(settings.EncryptionType);
                return encryptor.Encrypt(stream.ToArray(), initVector, settings.Password);
            }
        }
    }


    public static class MemoryStreamExtensions
    {
        public static void WriteShort(this MemoryStream stream, short value)
        {
            byte[] shortBuf = new byte[2];
            shortBuf[1] = (byte)(value & 0xff);
            shortBuf[0] = (byte)((value >> 8) & 0xff);
            stream.Write(shortBuf, 0, shortBuf.Length);
        }

        public static void WriteInt(this MemoryStream stream, int value)
        {
            byte[] intBuf = new byte[4];
            intBuf[3] = (byte)(value & 0xff);
            intBuf[2] = (byte)((value >> 8) & 0xff);
            intBuf[1] = (byte)((value >> 16) & 0xff);
            intBuf[0] = (byte)((value >> 24) & 0xff);

            stream.Write(intBuf, 0, intBuf.Length);
        }

        public static void WriteFixedString(this MemoryStream stream, string value, int size)
        {
            if(value == null)    
                return;

            var b = new byte[size];

            if (value.Length == 0)
            {
                stream.Write(b, 0, b.Length);
                return;
            }

            if (value.Length > size)
                value = value.Substring(0, size);

            var buffer = Encoding.ASCII.GetBytes(value);
            Buffer.BlockCopy(buffer, 0, b, 0, Math.Min(size, buffer.Length));

            stream.Write(b, 0, b.Length);
        }
    }
}