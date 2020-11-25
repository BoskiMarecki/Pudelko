using System;
using System.Collections.Generic;
using PudelkoLib;

namespace PudelkoConsoleApp
{
    public static class PudelkoExtation
    {
        public static Pudelko Compress(this Pudelko pudelko)
        {
            var side = Math.Cbrt(pudelko.Objetosc);
            return new Pudelko(side, side, side);
        }
    }

    class Program
    {
        static void Main()
        {

            Console.WriteLine("Pudelko Marek Woźniak");

            Pudelko p1 = new Pudelko(5, 7, 4);
            Pudelko p2 = new Pudelko(40, 50, 50, UnitOfMeasure.centimeter);
            Pudelko p3 = new Pudelko(100, 300, 500, UnitOfMeasure.centimeter);
            var pudelka = new Pudelko[] { p3, p2, p1 };

            Console.WriteLine("\nForeach: ");
            foreach (var p in pudelka)
            {
                Console.WriteLine(p);
            }

            Console.WriteLine("\nSort: ");
            Array.Sort(pudelka,Pudelko.CompareBoxes);

            foreach (var p in pudelka)
            {
                Console.WriteLine(p);
            }


            Console.WriteLine("\nNasze p1: "+ p1);
            Console.WriteLine("Nasze p2: "+ p2);

            Console.WriteLine("\nPole p1: "+ p1.Pole);
            Console.WriteLine("Objetosc p2: "+ p2.Objetosc);
            Console.WriteLine("\nCzy p1 jest równe p2 :"+ p1.Equals(p2));
            Console.WriteLine("GetHashCode p1:" + p1.GetHashCode());
            Console.WriteLine("GetHashCode p2:" + p2.GetHashCode());

            Console.WriteLine("\nPudełko sześcienne o takiej samej objętości, jak p: " + p1.Compress());
            Console.WriteLine("p1[1]: "+ p1[1]);
            Console.WriteLine("\nPrzeglądanie długości krawędzi: ");
            foreach(var i in p1)
            {
                Console.WriteLine(i);
            }
            Console.Write("Parse: ");
            Console.WriteLine(new Pudelko(2.5, 9.321, 0.1) == Pudelko.Parse("2.500 m × 9.321 m × 0.100 m"));




        }
    }
}
