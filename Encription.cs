using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace IBIZApp
{
    public class Encription
    {
        #region Euklidesz
        public static BigInteger Euclides
            (BigInteger a, BigInteger b) => b != 0 ? Euclides(b,a%b) : a;

        public static List<BigInteger> ExtEuclides(BigInteger a, BigInteger b)
        {
            var x0 = BigInteger.One;
            var x1 = BigInteger.Zero;
            var y0 = BigInteger.Zero;
            var y1 = BigInteger.One;
            var n = BigInteger.One;
            BigInteger x = x1;
            BigInteger y = y1;

            while (!b.IsZero)
            {
                var m = a % b;
                var q = a / b;
                a = b;
                b = m;
                x = x1;
                y = y1;

                x1 = BigInteger.Add(BigInteger.Multiply(x1,q),x0);
                y1 = BigInteger.Add(BigInteger.Multiply(y1, q), y0);
                x0 = x;
                y0 = y;

                if (n.Sign == 1)
                    BigInteger.Negate(n);
                else
                    BigInteger.Abs(n);
            }
            x = BigInteger.Multiply(n,x0);
            y = BigInteger.Multiply(n,y0);

            return new List<BigInteger> {a, x, y};
        }
        #endregion

        public static BigInteger Power(BigInteger x, BigInteger y)
        {
            BigInteger res = BigInteger.One;     // Initialize result

            while (y > 0)
            {
                // If y is odd, multiply x with result
                if ((y & 1) != 0)
                    res = BigInteger.Multiply(res,x);

                // y must be even now
                y = y >> 1; // y = y/2
                x = BigInteger.Multiply(x,x);
            }
            return res;
        }
        public static BigInteger ModPow(BigInteger a, BigInteger e, BigInteger m)
           => BigInteger.ModPow(a, e, m);
       

        public static BigInteger MyModPow(BigInteger a, BigInteger e, BigInteger m)
        {
            BigInteger result = BigInteger.One;
            BigInteger apow = a;

            while (e != 0)
            {
                if ((e & 0x01) == 0x01)
                    result = BigInteger.Multiply(result,apow) % m;
                e >>= 1;
                apow = BigInteger.Multiply(apow,apow) % m;
            }
            return result;
        }

        public static BigInteger ModInverse(BigInteger a, BigInteger m)
        {
            var m0 = m;
            var x = BigInteger.One;
            var y = BigInteger.Zero;

            if (m == BigInteger.One)
                return 0;
            while(a > BigInteger.One)
            {
                var q = BigInteger.Divide(a, m);
                var b = y;
                a = b;
                //a 2 következő sor lehet, hogy fordítva kell
                y = BigInteger.Multiply(BigInteger.Subtract(x, q), y);
                x = b;
            }
            if (x < 0)
                x += m0;

            return x;

            

        }

    }
}
