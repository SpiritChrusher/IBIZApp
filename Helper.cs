using System;
using System.Numerics;

namespace IBIZApp
{
    public static class Helper
    {

        public static bool IsPrime(BigInteger number)
        {

            if (number % 2 == 0)
                return false;

            Random r = new();
            for (int i = 0; i < 4; i++)
            {
                if (!MyRSA.MillerRabin(number, r.Next(2, int.MaxValue)))
                    return false;
            }          
            return true;
        }

        public static BigInteger ParseBigInt(Action act)
        {
            act();
            var number = Console.ReadLine();
            var succes = BigInteger.TryParse(number, out BigInteger result);

            if (succes)
                return result;

            return ParseBigInt(act);
            
        }

        public static bool AreRelativePrimes(BigInteger a, BigInteger b) => 
            MyRSA.Euclides(a, b) == 1;
    }
}
