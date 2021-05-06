using System;
using System.Numerics;
using System.Runtime.InteropServices;
using IBIZApp;


MyRSA tryhard = new();//(35423, 1489661, 31577);

BigInteger thenumber = Helper.ParseBigInt(() => Console.WriteLine("Give me a number to encrypt!"));

BigInteger enrypted = tryhard.Encrypt(thenumber);

Console.WriteLine("Encrypted number: {0}", enrypted);

var decrypted = tryhard.Decrypt(enrypted);

Console.WriteLine($"{thenumber} euqals {decrypted} => {thenumber == decrypted}");



