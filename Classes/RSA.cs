using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Text;


namespace GameConsole.Classes
{
    public sealed class RSA
    {
        public RSA()
        {
            FileProxy = new FileProxy();
        }

        public RSA(IFileProxy fileProxy)
        {
            if (fileProxy == null)
            {
                throw new ArgumentNullException("fileProxy");
            }

            this.FileProxy = fileProxy;
        }

        public IFileProxy FileProxy { get; set; }

        public void AssignNewKey(string publicKeyPath, string privateKeyPath)
        {
            if (string.IsNullOrEmpty(publicKeyPath))
            {
                throw new ArgumentNullException("publicKeyPath");
            }

            if (string.IsNullOrEmpty(privateKeyPath))
            {
                throw new ArgumentNullException("privateKeyPath");
            }

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024))
            {
                rsa.PersistKeyInCsp = false;

                if (FileProxy.Exists(privateKeyPath) == true)
                {
                    FileProxy.Delete(privateKeyPath);
                }

                if (FileProxy.Exists(publicKeyPath) == true)
                {
                    FileProxy.Delete(publicKeyPath);
                }

                string publicKey = rsa.ToXmlString(false);
                FileProxy.Write(publicKeyPath, publicKey);
                FileProxy.Write(privateKeyPath, rsa.ToXmlString(true));
            }
        }

        public string EncryptData(string publicKeyPath, string data2Encrypt)
        {
            if (string.IsNullOrEmpty(publicKeyPath))
            {
                throw new ArgumentNullException("publicKeyPath");
            }

            if (string.IsNullOrEmpty(data2Encrypt))
            {
                throw new ArgumentNullException("data2Encrypt");
            }

            byte[] cipherbytes;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024))
            {
                rsa.PersistKeyInCsp = false;

                rsa.FromXmlString(FileProxy.Read(publicKeyPath));

                byte[] plainbytes = Encoding.UTF8.GetBytes(data2Encrypt);
                cipherbytes = rsa.Encrypt(plainbytes, false);
            }

            return Convert.ToBase64String(cipherbytes);
        }

        public string DecryptData(string privateKeyPath, string data2Decrypt)
        {
            if (string.IsNullOrEmpty(privateKeyPath))
            {
                throw new ArgumentNullException("privateKeyPath");
            }

            if (string.IsNullOrEmpty(data2Decrypt))
            {
                throw new ArgumentNullException("data2Decrypt");
            }

            byte[] plain;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024))
            {
                rsa.PersistKeyInCsp = false;

                byte[] encodedCipherText = Convert.FromBase64String(data2Decrypt);
                rsa.FromXmlString(FileProxy.Read(privateKeyPath));
                plain = rsa.Decrypt(encodedCipherText, false);
            }

            return Encoding.UTF8.GetString(plain);
        }
    }
}