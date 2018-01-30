using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

using kanban.tool.utils;

namespace kanban.tool.decrypt {
    public class DecryptTool {
        // singleton mode
        private static DecryptTool tool = null;
        private DecryptTool() {}
        public static DecryptTool getInstance() {
            if(tool == null) {
                tool = new DecryptTool();
            }
            return tool;
        }

        public bool decryptFile(string fileName, string key, string outputFileName) {
            if (!File.Exists(fileName)) {
                Console.WriteLine("Could not find: " + fileName);
                return false;
            }

            // read the string from file
            StreamReader streamReader = new StreamReader(fileName, Encoding.Default);
            string line;
            line = streamReader.ReadLine();
            line = line.ToUpper().Trim();
            streamReader.Close();

            // decrypt
            string plaintext = Utils.decryptString(line, key);
            if(plaintext == null) {
                return false;
            }

            // output result
            StreamWriter streamWriter = new StreamWriter(outputFileName, false);
            streamWriter.WriteLine(plaintext);
            streamWriter.Close();

            return true;
        }
    }
}