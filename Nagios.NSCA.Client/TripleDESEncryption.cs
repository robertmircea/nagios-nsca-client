using System;
using System.Security.Cryptography;
using System.Text;

namespace Nagios.NSCA.Client
{
    public class TripleDESEncryption : NSCAEncryptionBase
    {
        public override byte[] Encrypt(byte[] s, byte[] initVector, string password)
        {
            byte[] keyBytes = new byte[24];
            byte[] passwordBytes = Encoding.ASCII.GetBytes(password);
            Buffer.BlockCopy(passwordBytes, 0, keyBytes, 0, Math.Min(24, passwordBytes.Length));

            byte[] iv = new byte[8];
            Buffer.BlockCopy(initVector, 0, iv, 0, Math.Min(8, iv.Length));

            using (TripleDES crypto = new TripleDESCryptoServiceProvider())
            {
                crypto.Mode = CipherMode.CFB;
                crypto.Padding = PaddingMode.PKCS7;

                return crypto.CreateEncryptor(keyBytes, iv).TransformFinalBlock(s, 0, s.Length);                
            }
        }
    }
}