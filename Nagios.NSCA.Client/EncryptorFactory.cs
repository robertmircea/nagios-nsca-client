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
                case NSCAEncryptionType.Rijndael128:
                    return new AESEncryption(16);
                case NSCAEncryptionType.Rijndael192:
                    return new AESEncryption(24);
                case NSCAEncryptionType.Rijndael256:
                    return new AESEncryption(32);
                default:
                    throw new ArgumentOutOfRangeException("encryptionType");
            }
        }
    }
}