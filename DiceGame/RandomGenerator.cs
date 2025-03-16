using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace DiceGame
{
    public static class RandomGenerator
    {
        public static (string key, string HMAC) ComputeHMAC(string message)
        {
            byte[] keyBytes = GenerateRandomKey();
            string randomKey = ConvertToHexString(keyBytes);

            string hmacResult = ComputeHMACForMessage(message, keyBytes);

            return (randomKey.ToUpper(), hmacResult.ToUpper());
        }

        private static byte[] GenerateRandomKey()
        {
            byte[] keyBytes = new byte[32];
            RandomNumberGenerator.Fill(keyBytes);
            return keyBytes;
        }

        private static string ConvertToHexString(byte[] bytes)
        {
            return string.Concat(bytes.Select(b => b.ToString("x2")));
        }

        private static string ComputeHMACForMessage(string message, byte[] keyBytes)
        {
            var hmac = new HMac(new Sha3Digest(256));
            hmac.Init(new KeyParameter(keyBytes));

            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            hmac.BlockUpdate(messageBytes, 0, messageBytes.Length);

            byte[] hmacBytes = new byte[hmac.GetMacSize()];
            hmac.DoFinal(hmacBytes, 0);

            return ConvertToHexString(hmacBytes);
        }

        public static int GenerateRandomNumber(int range)
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] randomNum = new byte[4];
            rng.GetBytes(randomNum);
            int value = BitConverter.ToInt32(randomNum, 0);
            return Math.Abs(value % range);
        }

        public static int CalculateResult(int userInput, int computerInput, int range)
        {
            return (userInput + computerInput) % range;
        }
    }
}
