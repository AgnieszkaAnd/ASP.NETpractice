using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RoutingDemo.Models {
    public static class Hasher {
        private static byte[] GetHash(string inputString, string UserName) {
            string salt = CreateSalt(UserName);
            string saltAndPwd = String.Concat(inputString, salt);


            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(saltAndPwd));
        }

        public static string GetHashString(string inputString, string UserName) {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString, UserName))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        private static string CreateSalt(string UserName) {
            string username = UserName;
            byte[] userBytes;
            string salt;
            userBytes = ASCIIEncoding.ASCII.GetBytes(username);
            long XORED = 0x00;

            foreach (int x in userBytes)
                XORED = XORED ^ x;

            Random rand = new Random(Convert.ToInt32(XORED));
            salt = rand.Next().ToString();
            salt += rand.Next().ToString();
            salt += rand.Next().ToString();
            salt += rand.Next().ToString();
            return salt;
        }
    }
}
