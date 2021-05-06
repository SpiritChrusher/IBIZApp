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

MyRSA tryhard = new(541, 1009, 1409, 135);

Console.WriteLine("Add meg a titkosítandó számot");

BigInteger titkositando = BigInteger.Parse(Console.ReadLine());

BigInteger tikosított = tryhard.Encryptor(titkositando);

Console.WriteLine("A tiktosított szám: {0}", tikosított);


Console.WriteLine($"{titkositando} ?= {tryhard.Decryptor(tikosított)}");


/*Adott az alábbi bináris fa (melynek gyökerét a "gymut" mutató jelöli ki) és egy vektor (V):

            50
         /      \
     25            77
    /  \          /  \
  19    32      61    79
   \    / \     / \   /
   22  30 44  54  76 78
    \      \   \
    23     46  58

V={ 28, 44, 56, 64, 81 }

Adott az alábbi algoritmus:

function FELADAT(gymut, V)
1. for i <- 1 to méret(V) do
2.    k <- kivétel(BESZÚR(gymut, V[i]))
3. end for
4. TÖRÖL(gymut, 25)
5. return gymut
end function


Ahol a BESZÚR és TÖRÖL a Moodle-be feltöltött (6. gyakorlat)  pdf fájlban található algoritmusok között található keresöfába beszúró ill. törlő algoritmus.

Válaszolja meg az alábbi kérdéseket a fenti algoritmusra ill. az így módosuló fára vonatkozóan!

9/a feladat:	Mindösszesen hányszor fut le a 8. sora a BESZÚR eljárásnak? (1 pont)

9/b feladat:	Milyen sorrendben dolgozza fel az elemeket a fa postorder bejárása? (1 pont)

9/c feladat:	Van-e legalább 5 elemű szigorúan bináris részfa? (1 pont)

9/d feladat:	Hány eleme van a legnagyobb elemszámú tökéletesen kiegyensúlyozott részfának? (1 pont)

*/
/* unsafe 
{
    int[] Vektor = { 28, 44, 56, 64, 81 };
    faelem gy = new faelem();
    gy.adat = 50;
    faelem* gymut = &gy;
    faelem fa = new();

    fa.Feladat(gymut, Vektor);
}
Console.WriteLine("end");
*/
unsafe struct faelem
{
    public int adat;
    public faelem* bal;
    public faelem* jobb;

    public faelem* gyoker;

   /* public faelem(int ertek)
    {
        adat = ertek;
        bal = null;
        jobb = null;
        gyoker = null;
    }*/
    public faelem* Feladat(faelem* rootptr, int[] V)
    {
        for (int i = 0; i < V.Length; i++)
        {
            beszur_reszfaba(rootptr,V[i]);
        }
        //TOROL(gymut, 25)
        return rootptr;
    }

    public void Torol(faelem* rootptr,int szam)
    {
        if (rootptr == null)
            return;
        if (rootptr->adat > szam)
            rootptr->bal = null;

        if (rootptr->adat < szam)
            rootptr->jobb = null;

        else
        {
            if (rootptr->jobb == null && rootptr->bal == null)
                rootptr = null;

            if (rootptr->jobb != null && rootptr->bal == null)
            {
                rootptr = rootptr->jobb;
            }
            if (rootptr->jobb == null && rootptr->bal != null)
            {
                rootptr = rootptr->bal;
            }
            if (rootptr->jobb != null && rootptr->bal != null)
            {
                /*Ha a gyökérelemnek két rákövetkez ̋oje van, akkor 
                 * agyökérelem értékét felülírjuk a gyökérelem bal oldali 
                 * részfájalegjobboldalibb elemének az értékével, majd 
                 * a gyökérelembal oldali részfájából töröljük ezt a legjobboldalibb elemet.*/
                return;
            }

            
        }
    }

    public void beszur(int ertek)
    {
        if (gyoker == null)
        {
            int size = Marshal.SizeOf(typeof(faelem));

            faelem* uj = (faelem*)Marshal.AllocHGlobal(size);
          
            uj->bal = null;
            uj->jobb = null; 
            uj->adat = ertek;
            gyoker = uj; } 
        else 
        { 
            beszur_reszfaba(gyoker, ertek); 
        } 
    }

    public void beszur_reszfaba(faelem* akt, int ertek) 
    { 
        if (ertek == akt->adat)
        {/*mar letezik ilyen kulcsu elem*/
                return; 
        }
        else 
        { 
            if (ertek < akt->adat) 
            { 
                if (akt->bal != null) 
                { 
                    beszur_reszfaba(akt->bal, ertek); 
                } 
                else 
                { 
                    faelem* uj = (faelem*)Marshal.SizeOf(typeof(faelem));
                    uj->bal = uj->jobb = null; 
                    akt->bal = uj;
                    uj->adat = ertek; 
                } 
            } 
            else/*if ertek > akt->adat*/
            { 
                if (akt->jobb != null) 
                { 
                    beszur_reszfaba(akt->jobb, ertek); 
                } 
                else 
                { 
                    faelem* uj = (faelem*) Marshal.SizeOf(typeof(faelem));
                    uj->adat = ertek; 
                    uj->bal = uj->jobb = null;
                    akt->jobb = uj; 
                } 
            } 
        } 
    }
}

