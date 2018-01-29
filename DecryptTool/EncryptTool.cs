using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace kanban.tool.encrypt {
    public class EncryptTool {
        // singleton mode
        private static EncryptTool tool = null;
        private EncryptTool() {}
        public static EncryptTool getInstance() {
            if(tool == null) {
                tool = new EncryptTool();
            }
            return tool;
        }

        // expend key to encrypt string
        private readonly static string DEFAULT_KEY = "damn it!"; 
        private static string keyExpend(string key) {
            // repeat the key to 16 bytes (string of 8 characters)
            if(key.Length == 0) {
                return EncryptTool.DEFAULT_KEY;
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

        public void encryptFile(string fileName, string key, string outputFileName) {
            // read the string from file
            StreamReader streamReader = new StreamReader(fileName, Encoding.Default);
            string line;
            StringBuilder builder = new StringBuilder();
            while ((line = streamReader.ReadLine()) != null) {
                builder.Append(line + "\r\n");
            }
            streamReader.Close();

            // encrypt string
            byte[] inputByteArray = Encoding.Default.GetBytes(builder.ToString());
            key = EncryptTool.keyExpend(key);
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

            // output result
            StreamWriter streamWriter = new StreamWriter(outputFileName, false);
            streamWriter.WriteLine(encryptedBuilder.ToString());
            streamWriter.Close();
        }
        public static void Main() {
            EncryptTool tool = EncryptTool.getInstance();
            tool.encryptFile("config.json", "neko go!", "encrypted.dat");
        }
    }
}