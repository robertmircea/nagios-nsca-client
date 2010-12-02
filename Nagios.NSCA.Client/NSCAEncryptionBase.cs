namespace Nagios.NSCA.Client
{
    public abstract class NSCAEncryptionBase
    {
        public abstract byte[] Encrypt(byte[] s, byte[] initVector, string password);
    }
}