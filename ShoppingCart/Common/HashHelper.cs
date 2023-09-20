using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ShoppingCart.Common
{
    public class HashHelper
    {
        private const string EncryptionKey = "shoppingcart123";
        private const string Salt = "shoppingcart888"; // 需要替換為你的鹽值
        private const int KeySize = 256;

        public static string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.KeySize = KeySize;
                aesAlg.BlockSize = 128;
                aesAlg.Mode = CipherMode.CFB;
                aesAlg.Padding = PaddingMode.PKCS7;

                byte[] saltBytes = Encoding.UTF8.GetBytes(Salt);
                using (var keyDerivationFunction = new Rfc2898DeriveBytes(EncryptionKey, saltBytes, 1000))
                {
                    byte[] keyBytes = keyDerivationFunction.GetBytes(KeySize / 8);
                    aesAlg.Key = keyBytes;

                    // 生成一個符合區塊大小的 IV
                    aesAlg.GenerateIV();

                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length); // 寫入 IV
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(plainText);
                            }
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.KeySize = KeySize;
                aesAlg.BlockSize = 128;
                aesAlg.Mode = CipherMode.CFB;
                aesAlg.Padding = PaddingMode.PKCS7;

                byte[] saltBytes = Encoding.UTF8.GetBytes(Salt);
                using (var keyDerivationFunction = new Rfc2898DeriveBytes(EncryptionKey, saltBytes, 1000))
                {
                    byte[] keyBytes = keyDerivationFunction.GetBytes(KeySize / 8);
                    aesAlg.Key = keyBytes;

                    byte[] cipherBytes = Convert.FromBase64String(cipherText);
                    using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                    {
                        byte[] iv = new byte[aesAlg.IV.Length];
                        msDecrypt.Read(iv, 0, iv.Length);

                        aesAlg.IV = iv;

                        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
    }
}