using System;

namespace Nagios.NSCA.Client
{
    public class EncryptorFactory
    {
        public static NSCAEncryptionBase CreateEncryptor(NSCAEncryptionType encryptionType)
        {
            switch (encryptionType)
            {
                case NSCAEncryptionType.Xor:
                    return new XorEncryption();
                case NSCAEncryptionType.TripleDES:
                    return new TripleDESEncryption();
                case NSCAEncryptionType.None:
                    return new NoEncryption();
                default:
                    throw new ArgumentOutOfRangeException("encryptionType");
            }
        }
    }
}