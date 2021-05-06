using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace IBIZApp
{

    public static class Helper
    {

        public static bool PrimE(BigInteger szam)
        {

            if (szam % 2 == 0)
                return false;

            Random r = new();
            for (int i = 0; i < 4; i++)
            {
                if (!MyRSA.MillerRabin(szam, r.Next(2,int.MaxValue)))
                    return false;
            }          
            return true;
        }

        public static BigInteger ParseBigInt(Action act)
        {
            act();
            var szam = Console.ReadLine();
            var succes = BigInteger.TryParse(szam,out BigInteger result);

            if (succes)
                return result;

            return ParseBigInt(act);
            
        }

        public static bool AreRelativePrimes(BigInteger a, BigInteger b) => MyRSA.Euclides(a, b) == 1;
    }

    public class MyRSA
    {
        public BigInteger P { get; init; }
        public BigInteger Q { get; init; }
        BigInteger N { get; init; }
        BigInteger PhiN { get; init; }
        BigInteger E { get; init; }
        BigInteger D { get; init; }

        public MyRSA()
        {
            P = SetNum(Helper.ParseBigInt(() => Console.WriteLine("Írja be a P értékét!")), 
                Helper.PrimE, () => Console.WriteLine("Írja be a P értékét!"));

            Q = SetNum(Helper.ParseBigInt(() => Console.WriteLine("Írja be a Q értékét!")), 
                Helper.PrimE, () => Console.WriteLine("Írja be a Q értékét!"));

            N = P * Q;
            PhiN = (P - 1) * (Q - 1);

            E = SetNum2(Helper.ParseBigInt(() => Console.WriteLine("Írja be az E értékét!")),
                PhiN, (e, PhiN) => Euclides(e, PhiN) == 1 ? true : false, () => Console.WriteLine("Írja be az E értékét!"));

            D = ModInverse(E, PhiN);
        }

        public MyRSA(BigInteger p, BigInteger q, BigInteger e)
        {
            P = SetNum(p, Helper.PrimE, () => Console.WriteLine("Írja be a P érétkét!"));
            Q = SetNum(q, Helper.PrimE, () => Console.WriteLine("Írja be a Q értékét!"));
            N = P * Q;
            PhiN = (P - 1) * (Q - 1);
            E = SetNum2(e, PhiN, (e, PhiN) =>  Euclides(e, PhiN) == 1 ? true : false, () => Console.WriteLine("Írja be az E értékét!"));
            D = ModInverse(e, PhiN);
        }

        private BigInteger SetNum(BigInteger number, Func<BigInteger,bool> Testfunc, Action act)
        {
            if (Testfunc(number))
               return number;

            var a = Helper.ParseBigInt(act);           
            return SetNum(a, Testfunc, act);
            

        }
        private BigInteger SetNum2(BigInteger number, BigInteger num2, Func<BigInteger, BigInteger, bool> Testfunc, Action act)
        {
            if (Testfunc(number, num2))
                return number;

            var a = Helper.ParseBigInt(act);
                
            return SetNum2(a, num2, Testfunc, act);


        }

        
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
                var Q = a / b;
                a = b;
                b = m;
                x = x1;
                y = y1;

                x1 = BigInteger.Add(BigInteger.Multiply(x1,Q),x0);
                y1 = BigInteger.Add(BigInteger.Multiply(y1, Q), y0);
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
            BigInteger res = BigInteger.One;

            while (y > 0)
            {
                if ((y & 1) != 0)
                    res = BigInteger.Multiply(res,x);

                y = y >> 1;
                x = BigInteger.Multiply(x,x);
            }
            return res;
        }
       /* public static BigInteger BasicModPow(BigInteger a, BigInteger e, BigInteger m)
           => BigInteger.ModPow(a, e, m);*/
       

        public static BigInteger ModPow(BigInteger a, BigInteger e, BigInteger m)
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
                var b = m;
                m = a % m;
                a = b;
                b = y;
                y = BigInteger.Subtract(x,BigInteger.Multiply(q, y));
                x = b;
            }

            if (x < 0)
                x += m0;

            return x;
        }

        public static BigInteger Kínai_Maradék_Tétel(BigInteger[] m, BigInteger[] c)
        {
            BigInteger M = m[0];
            BigInteger[] Mi = new BigInteger[m.Length];
            BigInteger[] Yi = new BigInteger[m.Length];
            for (int i = 1; i < m.Length; i++)
            {
                M *= m[i];
            }

            BigInteger x = 0;
            for (int i = 0; i < c.Length; i++)
            {
                Mi[i] = M / m[i];
                Yi[i] = ModInverse(Mi[i], m[i]);
                x += c[i] * Yi[i] * Mi[i];
            }

            x = x % M;

            return x;
        }

        public static bool Fermat_Teszt(long n, long k)
        {
            if (n <= 1 || n == 4)
                return false;
            if (n <= 3)
                return true;

            while (k > 0)
            {
                Random r = new();
                var a = r.Next(2, (int)n - 2)*2;

                if (Pow(a,n-1)%n != 1 || Euclides(a,n)  != 1)
                    return false;
            k--;
            }
            return true;
        }

        public static bool MillerRabin(BigInteger num, BigInteger a)
        {
            BigInteger m = num - 1;

            if (num <= 1 || num == 4)
                return false;
            if (num <= 3) 
                return true;

            do
            {
                m /= 2;
            } while (m % 2 == 0);

            var x = ModPow(a, m, num);

            if (x == 1 || x == num - 1)
            {
                return true;
            }

            while (m != num - 1)
            {
                x = (ModPow(x, 2, num));
                m = m * 2;

                if (x == 1) return false;
                if ((int)x == num - 1) return true;
            }
            return false;
        }

       
       /*long mérettel működik maximum 
        public static bool Miller_Rabin_Test(BigInteger n, BigInteger k)
        {
            if (n <= 1 || n == 4)
                return false;
            if (n <= 3)
                return true;
            var m = n - 1;

            while (m%2 == 0)
            {
                m = m / 2;
                for (int i = 0; i < k; i++)
                {
                    Random r = new();

                    var a = r.Next(2, (int)n - 2)*2;
                    var x = ModPow(a, m, n);

                    if (x == 1 || x == n - 1)
                        continue;

                    while (m != n-1)
                    {
                        x = BigInteger.Pow(x,2) % n;
                        m = m * 2;

                        if (x == 1)
                            return false;

                        if (x == n - 1)
                            continue;
                    }
                }
                return false;
            }
            return true;
        }
       */
        public BigInteger Encrypt(BigInteger num) => ModPow(num, E, N);


        public BigInteger Decrypt(BigInteger num)
        {

            BigInteger P_KMT = ModPow(num % P, D % (P - 1), P);
            BigInteger Q_KMT = ModPow(num % Q, D % (Q - 1), Q);

            BigInteger[] m = { P, Q };
            BigInteger[] c = { P_KMT, Q_KMT };

            return Kínai_Maradék_Tétel(m, c);
        }

    }
}
