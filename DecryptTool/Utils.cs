using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace kanban.tool.utils {
    public class Utils {

        // expend key to encrypt/decrypt string
        private readonly static string DEFAULT_KEY = "damn it!"; 
        private static string keyExpend(string key) {
            // repeat the key to 16 bytes (string of 8 characters)
            if(key == null || key.Length == 0) {
                return Utils.DEFAULT_KEY;
            }
            StringBuilder builder = new StringBuilder();
            int expenedLength = 8;
            while(builder.ToString().Length < expenedLength) {
                if(builder.ToString().Length + key.Length <= expenedLength) {
                    builder.Append(key);
                } else {
                    int remainingLength = expenedLength - builder.ToString().Length;
                    builder.Append(key.Substring(0, remainingLength));
                }
            }
            return builder.ToString();
        }

        public static string encryptString(string text, string key) {
            if(text == null || text.Length == 0) {
                return null;
            }
            // encrypt string
            byte[] inputByteArray = Encoding.Default.GetBytes(text);
            key = Utils.keyExpend(key);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = ASCIIEncoding.ASCII.GetBytes(key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
            cryptoStream.FlushFinalBlock();
            StringBuilder encryptedBuilder = new StringBuilder();
            foreach (byte b in memoryStream.ToArray()) {
                encryptedBuilder.AppendFormat("{0:X2}", b);
            }
            memoryStream.Close();
            cryptoStream.Close();
            return encryptedBuilder.ToString();
        }

        public static string decryptString(string text, string key) {
            // convert text to byte array
            // bad format
            if(text == null || text.Length == 0) {
                return null;
            }
            if(text.Length % 2 == 1) {
                return null;
            }
            byte[] inputByteArray = new byte[text.Length / 2];
            int inputCount = 0;
            string convertHelper = "0123456789ABCDEF";
            for(int i = 0; i < text.Length; i += 2) {
                char high = text[i], low = text[i + 1];
                int highNumber = convertHelper.IndexOf(high);
                int lowNumber = convertHelper.IndexOf(low);
                if(highNumber < 0 || lowNumber < 0) {
                    // bad format
                    return null;
                }
                inputByteArray[inputCount++] = (byte) (16 * highNumber + lowNumber);
            }

            // decrypt string
            key = Utils.keyExpend(key);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = ASCIIEncoding.ASCII.GetBytes(key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
            cryptoStream.FlushFinalBlock();
            string plaintext = System.Text.Encoding.Default.GetString(memoryStream.ToArray());
            memoryStream.Close();
            cryptoStream.Close();
            return plaintext;
        }

        public static string MD5Digest(string text) {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.Default.GetBytes(text);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++) {
                byte2String += targetData[i].ToString("x");
            }
            return byte2String.ToUpper();
        }
    }
}