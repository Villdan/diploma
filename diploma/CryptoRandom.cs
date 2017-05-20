using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace diploma
{
    class CryptoRandom
    {
       private static RandomNumberGenerator _r;
       public CryptoRandom()
        {
            _r = RandomNumberGenerator.Create();
        }
      public void GetBytes(byte[] buffer)
        {
            _r.GetBytes(buffer);
        }
       public double NextDouble()
        {
            byte[] b = new byte[4];
            _r.GetBytes(b);
            return (double)BitConverter.ToUInt32(b, 0) / uint.MaxValue;
        }
        public int Next(int minValue, int maxValue)
        {
            return (int)Math.Round(NextDouble() * (maxValue - minValue - 1)) + minValue;
        }
        public int Next()
        {
            return Next(0, int.MaxValue);
        }
        public int Next(int maxValue)
        {
            return Next(0, maxValue);
        }
    }
}
