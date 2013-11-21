using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Nagios.NSCA.Client
{
    public class AESEncryption : NSCAEncryptionBase
    {
        private int keyByteLength;

        public AESEncryption(int keyByteLength)
        {
            this.keyByteLength = keyByteLength;
        }

        public override byte[] Encrypt(byte[] s, byte[] initVector, string password)
        {
            byte[] keyBytes = new byte[keyByteLength];
            byte[] passwordBytes = Encoding.ASCII.GetBytes(password);
            Buffer.BlockCopy(passwordBytes, 0, keyBytes, 0, Math.Min(keyByteLength, passwordBytes.Length));

            byte[] iv = new byte[keyByteLength];
            Buffer.BlockCopy(initVector, 0, iv, 0, Math.Min(keyByteLength, iv.Length));

            using (RijndaelManaged crypto = new RijndaelManaged())
            {
                crypto.BlockSize = keyByteLength * 8;
                crypto.Mode = CipherMode.CFB;
                crypto.FeedbackSize = 8;
                crypto.Padding = PaddingMode.Zeros;

                var cryptoBytes = crypto.CreateEncryptor(keyBytes, iv).TransformFinalBlock(s, 0, s.Length);
                return cryptoBytes;
            }
        }
    }
}
