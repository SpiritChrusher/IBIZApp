using System;
using System.Numerics;
using System.Runtime.InteropServices;
using IBIZApp;

Console.WriteLine(MyRSA.Euclides(70,5));
var list = MyRSA.ExtEuclides(112,63);
list.ForEach(x => global::System.Console.WriteLine(x));

Console.WriteLine(4151%53);
BigInteger.DivRem(4151, 53, out var rem);
Console.WriteLine(rem);


MyRSA tryhard = new();//(35423, 1489661, 31577);

BigInteger thenumber = Helper.ParseBigInt(() => Console.WriteLine("Give me a number to encrypt!"));

BigInteger enrypted = tryhard.Encrypt(thenumber);

Console.WriteLine("Encrypted number: {0}", enrypted);

var decrypted = tryhard.Decrypt(enrypted);

Console.WriteLine($"{thenumber} euqals {decrypted} => {thenumber == decrypted}");



