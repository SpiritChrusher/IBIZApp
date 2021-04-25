using System;
using System.Numerics;
using IBIZApp;

Console.WriteLine(Encription.Euclides(70,5));
var list = Encription.ExtEuclides(112,63);
list.ForEach(x => global::System.Console.WriteLine(x));

Console.WriteLine(4151%53);
BigInteger.DivRem(4151, 53, out var rem);
Console.WriteLine(rem);
