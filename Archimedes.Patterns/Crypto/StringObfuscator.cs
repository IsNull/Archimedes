using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Crypto
{
    /// <summary>
    /// Obfuscates a string, to make it hard to read.
    /// Note that this is NOT an encryption in any way to secure data!
    /// </summary>
    public class StringObfuscator
    {
        public StringObfuscator() { }

        public string Obfuscate(string plaintext, byte key) {
            byte[] data = StringToByteArray(plaintext);
            for (int i = 0; i < data.Length; i++) {
                data[i] = (byte)(data[i] ^ key);
            }
            return System.Convert.ToBase64String(data);
        }

        public string DeObfuscate(string cryptotext, byte key) {
            byte[] data = System.Convert.FromBase64String(cryptotext);
            for (int i = 0; i < data.Length; i++) {
                data[i] = (byte)(data[i] ^ key);
            }
            return ByteArrayToString(data);
        }

        byte[] StringToByteArray(string str) {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetBytes(str);
        }

        string ByteArrayToString(byte[] arr) {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetString(arr);
        }
    }
}
