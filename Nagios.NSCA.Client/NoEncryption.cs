namespace Nagios.NSCA.Client
{
    public class NoEncryption : NSCAEncryptionBase
    {
        public override byte[] Encrypt(byte[] s, byte[] initVector, string password)
        {
            return s;
        }
    }
}