using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Drawing;

namespace kanban.main.utils {
    public class Utils {
        public static Color getLightColorFromString(string text) {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.Default.GetBytes(text);
            byte[] targetData = md5.ComputeHash(fromData);
            int p0 = targetData[0];
            int p1 = targetData[1 % targetData.Length];
            int p2 = targetData[2 % targetData.Length];
            int p3 = targetData[3 % targetData.Length];
            int p4 = targetData[4 % targetData.Length];
            int p5 = targetData[5 % targetData.Length];
            int p6 = targetData[6 % targetData.Length];
            int p7 = targetData[7 % targetData.Length];
            int p8 = targetData[8 % targetData.Length];
            int r = 200 + (p0 * 128 * 128 + p3 * 128 + p6) % 56;
            int g = 200 + (p1 * 128 * 128 + p4 * 128 + p7) % 56;
            int b = 200 + (p2 * 128 * 128 + p5 * 128 + p8) % 56;
            return Color.FromArgb(r, g, b);
        }
    }
}