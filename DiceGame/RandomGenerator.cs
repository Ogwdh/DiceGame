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
            byte[] keyBytes = new byte[32];
            RandomNumberGenerator.Fill(keyBytes);
            string RandomKey = string.Concat(keyBytes.Select(b => b.ToString("x2")));

            var hmac = new HMac(new Sha3Digest(256));
            hmac.Init(new KeyParameter(keyBytes));
            
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            hmac.BlockUpdate(messageBytes, 0, messageBytes.Length);

            byte[] hmacBytes = new byte[hmac.GetMacSize()];
            hmac.DoFinal(hmacBytes, 0);

            string hmacResult = string.Concat(hmacBytes.Select(b => b.ToString("x2")));

            return (RandomKey, hmacResult);
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
