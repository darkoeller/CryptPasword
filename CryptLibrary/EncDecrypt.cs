using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptLibrary
{
    public class EncDecrypt
    {
        private readonly string _encryptionKey = "DE11091965";
        private string _paswword;

        public EncDecrypt(string pasword)
        {
            _paswword = pasword;
        }

        public string Encrypt()
        {
            var clearBytes = Encoding.Unicode.GetBytes(_paswword);
            using (var encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(_encryptionKey,
                    new byte[] {0x46, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
                if (encryptor == null) return _paswword;
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    _paswword = Convert.ToBase64String(ms.ToArray());
                }
            }
            return _paswword;
        }

        public string Decrypt()
        {
            var cipherBytes = Convert.FromBase64String(_paswword);
            using (var encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(_encryptionKey,
                    new byte[] {0x46, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
                if (encryptor == null) return _paswword;
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    _paswword = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return _paswword;
        }
    }
}