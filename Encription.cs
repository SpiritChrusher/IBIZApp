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
            int[] teszt_szamok = { 3, 7, 8 };
            for (int i = 0; i < teszt_szamok.Length; i++)
            {
                if (!MyRSA.MillerRabin2((long)szam, teszt_szamok[i])) 
                    return false;
            }
            return true;
        }

        public static BigInteger ParseBigInt()
        {
            Console.WriteLine("Egy számot kérlek:");
            var szam = Console.ReadLine();
            var succes = BigInteger.TryParse(szam,out BigInteger result);

            if (succes)
                return result;

            return ParseBigInt();
            
        }

        public static bool AreRelativePrimes(BigInteger a, BigInteger b) => MyRSA.Euclides(a, b) == 1;
    }

    public class MyRSA
    {
        public BigInteger P { get; init; }
        public BigInteger Q { get; init; }
        BigInteger PhiN { get; init; }
        BigInteger N { get; init; }
        BigInteger E { get; init; }
        BigInteger D { get; init; }

        public MyRSA(BigInteger p, BigInteger q, BigInteger e, BigInteger d)
        {
            P = SetNum(p, Helper.PrimE);
            Q = SetNum(q, Helper.PrimE);
            N = P * Q;
            PhiN = (P - 1) * (Q - 1);
            E = SetNum2(e, PhiN, (e, PhiN) =>  Euclides(e, PhiN) == 1 ? true : false);
            D = ModInverse(d, PhiN);
        }

        private BigInteger SetNum(BigInteger number, Func<BigInteger,bool> Testfunc)
        {
            if (Testfunc(number))
               return number;

            var a = Helper.ParseBigInt();
           
            return SetNum(a, Testfunc);
            

        }
        private BigInteger SetNum2(BigInteger number, BigInteger num2, Func<BigInteger, BigInteger, bool> Testfunc)
        {
            if (Testfunc(number, num2))
                return number;

            var a = Helper.ParseBigInt();
                
            return SetNum2(a, num2, Testfunc);


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
                var Q = BigInteger.Divide(a, m);
                var b = y;
                a = b;
                //a 2 következő sor lehet, hogy fordítva kell
                y = BigInteger.Multiply(BigInteger.Subtract(x, Q), y);
                x = b;
            }

            if (x < 0)
                x += m0;

            return x;
        }

        public static BigInteger KínaiMaradéktétel(BigInteger[] c, BigInteger[] m)
        {
            //kitalálok valami szebb módszert, 
            //mint egy kiírt foreach;
            BigInteger M = BigInteger.One;
            foreach (var item in m)
                M *= item;

            var x = BigInteger.Zero;

            for (int i = 0; i < c.Length; i++)
            {
                var Mi = BigInteger.Divide(M, m[i]);
                var Yi = ModInverse(Mi, m[i]);
                x += BigInteger.Multiply(BigInteger.Multiply(c[i],Mi),Yi);
            }
            BigInteger.DivRem(x,M, out BigInteger rem);

            return rem;
        }

        public static bool Fermat_Teszt(int n, int k)
        {
            if (n <= 1 || n == 4)
                return false;
            if (n <= 3)
                return true;

            while (k > 0)
            {
                var a = GenerateRandom(2, n - 2); //r.Next(2, n - 2);

                if (Pow(a,n-1)%n != 1 || Euclides(a,n)  != 1)
                    return false;
            k--;
            }
            return true;
        }

        //még kell Miller-Rabin és RSA

        public static bool MillerRabin2(long num, int a)
        {
            long m = num - 1;

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
                x = (ModPow((long)x, 2, num));
                m = m * 2;

                if (x == 1) return false;
                if ((int)x == num - 1) return true;
            }
            return false;
        }

        public static bool Miller_Rabin_Test(long n, long k)
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
                    var a = GenerateRandom(2, (int)n - 2);
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

        public BigInteger Encryptor(BigInteger input) => ModPow(input, E, N);


        public BigInteger Decryptor(BigInteger input)
        {

            BigInteger Q_to_crt = ModPow(input % Q, D % (Q - 1), Q);
            BigInteger P_to_crt = ModPow(input % P, D % (P - 1), P);

            BigInteger[] c = { P_to_crt, Q_to_crt };
            BigInteger[] m = { P, Q };

            return KínaiMaradéktétel(m, c);
        }

        private static int GenerateRandom(int min, int max)
        {
            Random random = new Random();
            byte[] data = new byte[max];
            random.NextBytes(data);
            var big = new BigInteger(data);

            Random r = new Random();
            return r.Next(min,max);
        }

    }
}
