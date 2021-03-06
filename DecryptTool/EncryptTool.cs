using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

using kanban.tool.utils;

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

        public bool encryptFile(string fileName, string key, string outputFileName) {
            if (!File.Exists(fileName)) {
                Console.WriteLine("Could not find: " + fileName);
                return false;
            }

            // read the string from file
            StreamReader streamReader = new StreamReader(fileName, Encoding.Default);
            string line;
            StringBuilder builder = new StringBuilder();
            while ((line = streamReader.ReadLine()) != null) {
                builder.Append(line + "\r\n");
            }
            streamReader.Close();

            // encypt
            string cypher = Utils.encryptString(builder.ToString(), key);
            if(cypher == null) {
                return false;
            }

            // output result
            StreamWriter streamWriter = new StreamWriter(outputFileName, false);
            streamWriter.WriteLine(cypher);
            streamWriter.Close();

            return true;
        }
    }
}